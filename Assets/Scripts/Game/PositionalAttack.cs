using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PositionalAttack", menuName = "Effects/PositionalAttack", order = 50)]
public class PositionalAttack : Effect
{
    [SerializeField] private GameObject visualPrefab;
    [SerializeField] private int damage = 1;
    [SerializeField] private float radius = 1;

    public override void Invoke(ActionData data)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(visualPrefab, mousePos, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, radius, data.targetMask);
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<Actor>(out Actor target))
            {
                target.TakeDamage(damage);
            }
        }
    }
}
