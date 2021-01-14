using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private const float ANIM_LOCK_DURATION = 0.05f;

    [System.Serializable] public class PositionChangedEvent : UnityEvent<Vector2> {}

    [SerializeField] private float moveSpeed = 1f;

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
    }

    void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(inputH, inputV).normalized;

        UpdateMoveAnimations(inputH, inputV);

        if (transform.position != prevFramePosition)
        {
            onPositionChanged.Invoke(transform.position);
        }

        prevFramePosition = transform.position;
        rigidbody2D.velocity = inputDir * moveSpeed;
    }

    private void UpdateMoveAnimations(float inputH, float inputV)
    {
        if (!isAnimLocked)
        {
            StartCoroutine(AnimationLock());
            animator.SetFloat("horizontal", inputH);
            animator.SetFloat("vertical", inputV);
        }
    }

    private IEnumerator AnimationLock()
    {
        isAnimLocked = true;
        yield return animLockInstruction;
        isAnimLocked = false;
    }
}
