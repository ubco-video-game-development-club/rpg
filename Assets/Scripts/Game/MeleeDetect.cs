using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MeleeDetect", menuName = "Effects/MeleeDetect", order = 51)]
public class MeleeDetect : Effect
{
    public int damage;
    public float reach;
    public float arcAngle;

    public override void Invoke(ActionData data)
    {
        Debug.Log("Invoked Cleave");
        Debug.Log("Origin: " + data.origin);

        // Calculate direction based on mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDir = (mousePos - data.origin).normalized;

        Debug.Log("Attack Dir: " + attackDir);

        // Check all colliders within the reach
        Collider2D[] hits = Physics2D.OverlapCircleAll(data.origin, reach, data.targetMask);
        foreach (Collider2D hit in hits)
        {
            // Calculate direction to the target
            Vector2 targetPos = hit.transform.position;
            Vector2 targetDir = (targetPos - data.origin).normalized;

            Debug.Log("Target Dir: " + targetDir);

            Debug.Log("Angle: " + Vector2.SignedAngle(attackDir, targetDir));

            // Determine whether the target is within the arcAngle
            if (Vector2.SignedAngle(attackDir, targetDir) < arcAngle / 2)
            {
                Debug.Log("Hit!");
                Debug.Log(hit.name);
                if (hit.TryGetComponent<Actor>(out Actor target))
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }
}
