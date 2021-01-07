using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Animator animator;

    void Awake()
    {
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
        Debug.Log(inputH);
        animator.SetFloat("horizontal", inputH);
        animator.SetFloat("vertical", inputV);
    }
}
