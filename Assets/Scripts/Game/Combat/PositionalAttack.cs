using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New PositionalAttack", menuName = "Actions/Positional Attack", order = 51)]
    public class PositionalAttack : Action
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
}
