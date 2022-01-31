using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New ProjectileAttack", menuName = "Actions/Projectile Attack", order = 52)]
    public class ProjectileAttack : Action
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private float damage;

        public override void Invoke(ActionData data)
        {
            // Calculate direction based on mouse position
            Vector2 attackDir = (data.target - data.origin).normalized;

            // Create projectile
            GameObject proj = Instantiate(projectilePrefab, data.origin, Quaternion.identity);
            Rigidbody2D rb2D = proj.GetComponent<Rigidbody2D>();
            rb2D.velocity = attackDir * speed;
        }
    }
}
