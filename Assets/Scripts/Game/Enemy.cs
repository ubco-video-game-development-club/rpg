using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] private float flashDuration = 0.3f;

    private YieldInstruction flashDurationInstruction;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
        flashDurationInstruction = new WaitForSeconds(flashDuration);
    }

    void Start()
    {
        OnHealthChanged.AddListener(FlashRed);
    }

    private void FlashRed(int health)
    {
        StartCoroutine(FlashRedTimer());
    }

    private IEnumerator FlashRedTimer()
    {
        spriteRenderer.color = Color.red;
        yield return flashDurationInstruction;
        spriteRenderer.color = Color.white;
    }
}
