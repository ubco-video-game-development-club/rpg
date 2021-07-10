using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public struct AbilitySlot
    {
        public bool enabled;
        public Action ability;
    }

    [RequireComponent(typeof(AnimatorCache))]
    public class Player : Actor
    {
        public const float GLOBAL_COOLDOWN = 0.25f;
        private const float ANIM_LOCK_DURATION = 0.05f;
        private const int MAX_ABILITY_SLOTS = 4;
        private const int MAX_INTERACT_TARGETS = 5;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Action primaryAttack;
        [SerializeField] private Action secondaryAttack;
        [SerializeField] private float interactRadius;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private Tooltip interactTooltipPrefab;

        public List<Action> AvailableAbilities { get; set; }
        public List<Upgrade> Upgrades { get; set; }
        public ClassBaseStats ClassBaseStats { get; set; }
        public AbilitySlot[] AbilitySlots { get; set; }
        public RuntimeAnimatorController BaseAnimationController { get; private set; }

        private bool isGCDActive;
        private bool isAnimLocked;
        private Vector3 prevFramePosition;
        private ActionData attackData;
        private Collider2D[] interactTargets = new Collider2D[MAX_INTERACT_TARGETS];
        private int numInteractTargets = 0;
        private Interactable targetInteractable;
        private Tooltip interactTooltip;

        private YieldInstruction animLockInstruction;
        private YieldInstruction globalCooldownInstruction;

        private Animator animator;
        private AnimatorCache animatorCache;
        private new Rigidbody2D rigidbody2D;

        private UnityEvent<Vector2> onPositionChanged = new UnityEvent<Vector2>();

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            animatorCache = GetComponent<AnimatorCache>();
            rigidbody2D = GetComponent<Rigidbody2D>();

            interactTooltip = Instantiate(interactTooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
            interactTooltip.SetTarget(transform);
            interactTooltip.SetActive(false);

            animLockInstruction = new WaitForSeconds(ANIM_LOCK_DURATION);
            globalCooldownInstruction = new WaitForSeconds(GLOBAL_COOLDOWN);
            BaseAnimationController = animator.runtimeAnimatorController;

            AvailableAbilities = new List<Action>();
            AbilitySlots = new AbilitySlot[MAX_ABILITY_SLOTS];
            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                AbilitySlots[i] = new AbilitySlot();
            }

            primaryAttack.Enabled = true;
            secondaryAttack.Enabled = true;

            attackData = new ActionData(LayerMask.GetMask("Enemy"));
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 160, 10, 150, Screen.height - 230));
            GUILayout.Label("Player Properties");
            foreach (KeyValuePair<PropertyName, dynamic> prop in Properties)
            {
                GUILayout.Label($"{prop.Key}: {prop.Value}");
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width - 160, Screen.height - 200, 150, 200));
            GUILayout.Label("Available Abilities");
            foreach (Action ability in AvailableAbilities)
            {
                int matchingIdx = -1, lastInactiveIdx = -1;
                for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
                {
                    if (AbilitySlots[i].ability == ability) matchingIdx = i;
                    if (lastInactiveIdx == -1 && !AbilitySlots[i].enabled) lastInactiveIdx = i;
                }
                if (matchingIdx == -1 && lastInactiveIdx == -1) GUI.enabled = false;
                bool isOn = GUILayout.Toggle(matchingIdx >= 0, $"{ability.name}");
                GUI.enabled = true;
                if (isOn && matchingIdx == -1 && lastInactiveIdx >= 0)
                {
                    AbilitySlots[lastInactiveIdx].enabled = true;
                    AbilitySlots[lastInactiveIdx].ability = ability;
                    AbilitySlots[lastInactiveIdx].ability.Enabled = true;
                }
                else if (!isOn && matchingIdx >= 0)
                {
                    AbilitySlots[matchingIdx].enabled = false;
                    AbilitySlots[matchingIdx].ability.Enabled = false;
                    AbilitySlots[matchingIdx].ability = null;
                }
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height - 60, 300, 50));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                string abilityText = "";
                if (AbilitySlots[i].enabled)
                {
                    abilityText = $"{AbilitySlots[i].ability.name}";
                    GUI.color = AbilitySlots[i].ability.Enabled ? Color.white : Color.red;
                }
                else GUI.enabled = false;
                GUILayout.Box(abilityText);
                GUI.color = Color.white;
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void Update()
        {
            // Handle movement inputs
            float inputH = Input.GetAxisRaw("Horizontal");
            float inputV = Input.GetAxisRaw("Vertical");
            Vector2 inputDir = new Vector2(inputH, inputV).normalized;

            if (inputH != 0 || inputV != 0) UpdateMoveAnimations(inputH, inputV);

            if (transform.position != prevFramePosition) onPositionChanged.Invoke(transform.position);

            prevFramePosition = transform.position;
            rigidbody2D.velocity = inputDir * moveSpeed;

            // Update interaction
            UpdateInteractions();
            if (Input.GetButtonDown("Interact") && targetInteractable != null)
            {
                targetInteractable.Interact(this);
            }

            // Global Cooldown: Anything after this point will not run while the GCD is active
            if (isGCDActive) return;

            // Handle attack inputs
            HandleAttackInput("Primary", primaryAttack);
            HandleAttackInput("Secondary", secondaryAttack);

            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                if (AbilitySlots[i].enabled)
                {
                    HandleAttackInput($"Ability{i + 1}", AbilitySlots[i].ability);
                }
            }
        }

        public void ClearAnimationOverrides()
        {
            // Clear any animator overrides caused by the current action
            animatorCache.CacheParameters();
            animator.runtimeAnimatorController = BaseAnimationController;
            animatorCache.RestoreParameters();
        }

        private IEnumerator GlobalCooldown()
        {
            isGCDActive = true;
            yield return globalCooldownInstruction;
            isGCDActive = false;
        }

        private void HandleAttackInput(string button, Action attack)
        {
            if (!isGCDActive && attack.Enabled && Input.GetButton(button))
            {
                StartCoroutine(GlobalCooldown());
                StartCoroutine(AttackCooldown(attack));

                UpdateAttackAnimations(attack);
                attackData.origin = transform.position;
                attack.Invoke(attackData);
            }
        }

        private void UpdateAttackAnimations(Action attack)
        {
            // Temporarily override the player's animation controller with this attack's controller
            animator.runtimeAnimatorController = attack.AnimationController;

            // Update animator parameters
            Vector2 mouseDir = GetMouseDirection();
            float xWeight = Mathf.Round(mouseDir.x);
            float yWeight = Mathf.Round(mouseDir.y);
            animator.SetFloat("hSpeed", xWeight);
            animator.SetFloat("vSpeed", yWeight);
            animator.SetFloat("hDirection", xWeight);
            animator.SetFloat("vDirection", yWeight);
            animator.SetTrigger("attack");
        }

        private IEnumerator AttackCooldown(Action attack)
        {
            attack.Enabled = false;
            yield return new WaitForSeconds(attack.Cooldown);
            attack.Enabled = true;
        }

        private void UpdateMoveAnimations(float inputH, float inputV)
        {
            if (!isAnimLocked)
            {
                StartCoroutine(AnimationLock());
                animator.SetFloat("hSpeed", inputH);
                animator.SetFloat("vSpeed", inputV);
            }
        }

        private IEnumerator AnimationLock()
        {
            isAnimLocked = true;
            yield return animLockInstruction;
            isAnimLocked = false;
        }

        private Vector2 GetMouseDirection()
        {
            Vector2 mouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            return mouseVector.normalized;
        }

        private void UpdateInteractions()
        {
            Interactable interactable;

            // Clear current interactions
            for (int i = 0; i < numInteractTargets; i++)
            {
                if (interactTargets[i].TryGetComponent<Interactable>(out interactable))
                {
                    interactable.SetTooltipActive(false);
                }
            }
            targetInteractable = null;
            interactTooltip.SetActive(false);

            // Check for nearby interactables
            numInteractTargets = Physics2D.OverlapCircleNonAlloc(transform.position, interactRadius, interactTargets, interactLayer);
            int closestIdx = -1;
            float smallestSqrDist = float.MaxValue;
            for (int i = 0; i < numInteractTargets; i++)
            {
                float sqrDist = (interactTargets[i].transform.position - transform.position).sqrMagnitude;
                if (sqrDist < smallestSqrDist)
                {
                    smallestSqrDist = sqrDist;
                    closestIdx = i;
                }
            }

            // Set the nearest interactable active
            if (closestIdx >= 0 && interactTargets[closestIdx].TryGetComponent<Interactable>(out interactable))
            {
                targetInteractable = interactable;
                interactable.SetTooltipActive(true);
                interactTooltip.SetActive(true);
            }
        }
    }
}
