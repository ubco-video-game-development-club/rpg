using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    private const float ANIM_LOCK_DURATION = 0.05f;

    [System.Serializable] public class PositionChangedEvent : UnityEvent<Vector2> { }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Attack primaryAttack;
    [SerializeField] private Attack secondaryAttack;

    private bool isAnimLocked;
    private Vector3 prevFramePosition;

    private YieldInstruction animLockInstruction;
    private Animator animator;
    private new Rigidbody2D rigidbody2D;

    private PositionChangedEvent onPositionChanged = new PositionChangedEvent();

    void Awake()
    {
        animLockInstruction = new WaitForSeconds(ANIM_LOCK_DURATION);
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

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

        // Handle attack inputs
        HandleAttackInput("Primary", primaryAttack);
        HandleAttackInput("Secondary", secondaryAttack);
    }

    private void HandleAttackInput(string button, Attack attack)
    {
        if (attack.Enabled && Input.GetButton(button))
        {
            StartCoroutine(AttackCooldown(attack));
            UpdateAttackAnimations();
            attack.Invoke();
        }
    }

    private void UpdateAttackAnimations()
    {
        Vector2 mouseDir = GetMouseDirection();
        float xWeight = Mathf.Round(mouseDir.x);
        float yWeight = Mathf.Round(mouseDir.y);
        animator.SetFloat("hSpeed", xWeight);
        animator.SetFloat("vSpeed", yWeight);
        animator.SetFloat("hDirection", xWeight);
        animator.SetFloat("vDirection", yWeight);
        animator.SetTrigger("attack");
    }

    private IEnumerator AttackCooldown(Attack attack)
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
