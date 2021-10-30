using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Animation;

namespace RPG
{
    public struct AbilitySlot
    {
        public bool enabled;
        public Action ability;
    }

    public class Player : Actor
    {
        private const float GLOBAL_COOLDOWN = 0.5f;
        private const float ANIM_LOCK_DURATION = 0.08f;
        private const int MAX_ABILITY_SLOTS = 4;
        private const int MAX_INTERACT_TARGETS = 5;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Animator2D weaponAnimator2D;
        [SerializeField] private Weapon defaultPrimaryWeapon;
        [SerializeField] private Weapon defaultSecondaryWeapon;
        [SerializeField] private float interactRadius;
        [SerializeField] private LayerMask interactLayer;

        public ClassBaseStats ClassBaseStats { get; private set; }

        private List<Action> availableAbilities;
        private AbilitySlot[] abilitySlots;
        private Dictionary<ItemSlot, Item> equipment;

        private Weapon PrimaryWeapon { get => ((Weapon)equipment[ItemSlot.Mainhand]); }
        private Weapon SecondaryWeapon { get => ((Weapon)equipment[ItemSlot.Offhand]); }

        private bool isGCDActive;
        private bool isAnimLocked;
        private Vector3 prevFramePosition;
        private Collider2D[] interactTargets = new Collider2D[MAX_INTERACT_TARGETS];
        private int numInteractTargets = 0;
        private Interactable targetInteractable;

        private YieldInstruction animLockInstruction;
        private YieldInstruction globalCooldownInstruction;

        private UnityEvent<Vector2> onPositionChanged = new UnityEvent<Vector2>();

        protected override void Awake()
        {
            base.Awake();

            animLockInstruction = new WaitForSeconds(ANIM_LOCK_DURATION);
            globalCooldownInstruction = new WaitForSeconds(GLOBAL_COOLDOWN);

            availableAbilities = new List<Action>();
            abilitySlots = new AbilitySlot[MAX_ABILITY_SLOTS];
            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                abilitySlots[i] = new AbilitySlot();
            }
            equipment = new Dictionary<ItemSlot, Item>();
            foreach (ItemSlot slot in Item.SlotTypes)
            {
                equipment.Add(slot, null);
            }
            Equip(ItemSlot.Mainhand, defaultPrimaryWeapon);
            Equip(ItemSlot.Offhand, defaultSecondaryWeapon);

            actionData = new ActionData(LayerMask.GetMask("Enemy"));
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
            foreach (Action ability in availableAbilities)
            {
                int matchingIdx = -1, lastInactiveIdx = -1;
                for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
                {
                    if (abilitySlots[i].ability == ability) matchingIdx = i;
                    if (lastInactiveIdx == -1 && !abilitySlots[i].enabled) lastInactiveIdx = i;
                }
                if (matchingIdx == -1 && lastInactiveIdx == -1) GUI.enabled = false;
                bool isOn = GUILayout.Toggle(matchingIdx >= 0, $"{ability.name}");
                GUI.enabled = true;
                if (isOn && matchingIdx == -1 && lastInactiveIdx >= 0)
                {
                    abilitySlots[lastInactiveIdx].enabled = true;
                    abilitySlots[lastInactiveIdx].ability = ability;
                    abilitySlots[lastInactiveIdx].ability.Enabled = true;
                }
                else if (!isOn && matchingIdx >= 0)
                {
                    abilitySlots[matchingIdx].enabled = false;
                    abilitySlots[matchingIdx].ability.Enabled = false;
                    abilitySlots[matchingIdx].ability = null;
                }
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height - 60, 300, 50));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                string abilityText = "";
                if (abilitySlots[i].enabled)
                {
                    abilityText = $"{abilitySlots[i].ability.name}";
                    GUI.color = abilitySlots[i].ability.Enabled ? Color.white : Color.red;
                }
                else GUI.enabled = false;
                GUILayout.Box(abilityText);
                GUI.color = Color.white;
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        protected override void Update()
        {
            // Handle movement inputs
            float inputH = Input.GetAxisRaw("Horizontal");
            float inputV = Input.GetAxisRaw("Vertical");
            Vector2 inputDir = new Vector2(inputH, inputV).normalized;

            // Update position
            prevFramePosition = transform.position;
            rigidbody2D.velocity = inputDir * moveSpeed;
            if (transform.position != prevFramePosition) onPositionChanged.Invoke(transform.position);

            // Run base Actor update
            base.Update();

            // Update interaction
            UpdateInteractions();
            if (Input.GetButtonDown("Interact") && targetInteractable != null)
            {
                targetInteractable.Interact(this);
            }

            // Global Cooldown: Anything after this point will not run while the GCD is active
            if (isGCDActive) return;

            // Handle attack inputs
            HandleActionInput("Primary", PrimaryWeapon.Attack);
            HandleActionInput("Secondary", SecondaryWeapon.Attack);

            for (int i = 0; i < MAX_ABILITY_SLOTS; i++)
            {
                if (abilitySlots[i].enabled)
                {
                    HandleActionInput($"Ability{i + 1}", abilitySlots[i].ability);
                }
            }
        }

        public void ApplyClassBaseStats(ClassBaseStats classBaseStats)
        {
            ClassBaseStats = classBaseStats;
            classBaseStats.ApplyTo(this);
        }

        public void AddAbility(Action ability)
        {
            availableAbilities.Add(Instantiate(ability));
        }

        public void Equip(ItemSlot slot, Item item)
        {
            item = Instantiate(item);
            if (equipment[slot] != null && slot != ItemSlot.Mainhand && slot != ItemSlot.Offhand)
            {
                UnEquip(slot);
            }
            equipment[slot] = item;
            item.ApplyTo(this);

            switch (slot)
            {
                case ItemSlot.Mainhand:
                case ItemSlot.Offhand:
                    Weapon weapon = (Weapon)item;
                    weapon.Attack.Enabled = true;
                    break;
            }
        }

        public void UnEquip(ItemSlot slot)
        {
            equipment[slot].RemoveFrom(this);
            equipment[slot].Drop(transform.position);
            equipment[slot] = null;

            switch (slot)
            {
                case ItemSlot.Mainhand:
                    Equip(ItemSlot.Mainhand, defaultPrimaryWeapon);
                    break;
                case ItemSlot.Offhand:
                    Equip(ItemSlot.Offhand, defaultSecondaryWeapon);
                    break;
            }
        }

        protected override void AnimateMove()
        {
            if (!isAnimLocked)
            {
                StartCoroutine(AnimationLock());
                base.AnimateMove();
            }
        }

        protected override void AnimateAction(Action action)
        {
            facingDirection = GetActionDirection();
            AnimationSet8D avatarAnim = action.Animation;
            if (action.UseWeaponAnimation)
            {
                WeaponAnimation weaponAnim = PrimaryWeapon.GetAnimation(action.AnimationType);
                avatarAnim = weaponAnim.AvatarAnimation;
                weaponAnimator2D.PlayAnimation(weaponAnim.Animation.Get(facingDirection), false, true);
            }
            animator2D.PlayAnimation(avatarAnim.Get(facingDirection), false);
        }

        private void HandleActionInput(string button, Action action)
        {
            if (!isGCDActive && action.Enabled && Input.GetButtonDown(button))
            {
                // Apply cooldowns
                StartCoroutine(GlobalCooldown());
                StartCoroutine(ActionCooldown(action));

                // Invoke action
                actionData.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                actionData.origin = transform.position;
                action.Invoke(actionData);

                // Run animations (requires action data to be set!)
                AnimateAction(action);
            }
        }

        private IEnumerator GlobalCooldown()
        {
            isGCDActive = true;
            yield return globalCooldownInstruction;
            isGCDActive = false;
        }

        private IEnumerator ActionCooldown(Action attack)
        {
            attack.Enabled = false;
            yield return new WaitForSeconds(attack.Cooldown);
            attack.Enabled = true;
        }

        private IEnumerator AnimationLock()
        {
            isAnimLocked = true;
            yield return animLockInstruction;
            isAnimLocked = false;
        }

        private void UpdateInteractions()
        {
            Interactable interactable;

            // Clear current interactions
            for (int i = 0; i < numInteractTargets; i++)
            {
                Collider2D target = interactTargets[i];
                if (target != null && target.TryGetComponent<Interactable>(out interactable))
                {
                    interactable.SetTooltipActive(false);
                }
            }
            targetInteractable = null;

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
            }
        }
    }
}
