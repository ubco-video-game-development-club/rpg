using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    public const float GLOBAL_COOLDOWN = 0.5f;
    private const float ANIM_LOCK_DURATION = 0.05f;

    [System.Serializable] public class PositionChangedEvent : UnityEvent<Vector2> { }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Action primaryAttack;
    [SerializeField] private Action secondaryAttack;

    private bool isGCDActive;
    private bool isAnimLocked;
    private Vector3 prevFramePosition;

    private YieldInstruction animLockInstruction;
    private YieldInstruction globalCooldownInstruction;
    private RuntimeAnimatorController baseAnimationController;
    private Animator animator;
    private new Rigidbody2D rigidbody2D;

    private PositionChangedEvent onPositionChanged = new PositionChangedEvent();

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        animLockInstruction = new WaitForSeconds(ANIM_LOCK_DURATION);
        globalCooldownInstruction = new WaitForSeconds(GLOBAL_COOLDOWN);
        baseAnimationController = animator.runtimeAnimatorController;

        primaryAttack.Enabled = true;
        secondaryAttack.Enabled = true;
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

        // Global Cooldown: Anything after this point will not run while the GDC is active
        if (isGCDActive) return;

        // Handle attack inputs
        HandleAttackInput("Primary", primaryAttack);
        HandleAttackInput("Secondary", secondaryAttack);
    }

    private IEnumerator GlobalCooldown()
    {
        isGCDActive = true;
        yield return globalCooldownInstruction;
        isGCDActive = false;

        // TODO: Carry animation parameters through controller updates

        // Clear any animator overrides caused by the current action
        animator.runtimeAnimatorController = baseAnimationController;
    }

    private void HandleAttackInput(string button, Action attack)
    {
        if (!isGCDActive && attack.Enabled && Input.GetButton(button))
        {
            StartCoroutine(GlobalCooldown());
            StartCoroutine(AttackCooldown(attack));
            UpdateAttackAnimations(attack);
            attack.Invoke();
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
}
