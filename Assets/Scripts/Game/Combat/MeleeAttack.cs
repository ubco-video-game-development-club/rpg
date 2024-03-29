using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New MeleeAttack", menuName = "Actions/Melee Attack", order = 50)]
    public class MeleeAttack : Action
    {
        [SerializeField] private int damage;
        [SerializeField] private float reach;
        [SerializeField] private float arcAngle;

        public override void Invoke(ActionData data)
        {
            // Calculate direction based on target
            Vector2 attackDir = (data.target - data.origin).normalized;

            // Check all colliders within the reach
            Collider2D[] hits = Physics2D.OverlapCircleAll(data.origin, reach, data.targetMask);
            foreach (Collider2D hit in hits)
            {
                // Calculate direction to the target
                Vector2 targetPos = hit.ClosestPoint(data.origin);
                Vector2 targetDir = (targetPos - data.origin).normalized;

                // Determine whether the target is within the arcAngle
                if (Vector2.Angle(attackDir, targetDir) < arcAngle / 2)
                {
                    if (hit.TryGetComponent<Actor>(out Actor target))
                    {
                        target.TakeDamage(damage, data.source);
                    }
                }
            }
        }
    }
}
