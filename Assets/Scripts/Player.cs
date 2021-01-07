using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float ANIM_LOCK_DURATION = 0.05f;

    [SerializeField] private float moveSpeed = 1f;

    private bool isAnimLocked;
    private YieldInstruction animLockInstruction;
    private Animator animator;

    void Awake()
    {
        animLockInstruction = new WaitForSeconds(ANIM_LOCK_DURATION);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(inputH, inputV).normalized;

        UpdateMoveAnimations(inputH, inputV);

        transform.position += inputDir * moveSpeed * Time.deltaTime;
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
