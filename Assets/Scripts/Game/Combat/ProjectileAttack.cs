using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New ProjectileAttack", menuName = "Effects/ProjectileAttack", order = 52)]
    public class ProjectileAttack : Effect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private float damage;

        public override void Invoke(ActionData data)
        {
            // Calculate direction based on mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 attackDir = (mousePos - data.origin).normalized;

            // Create projectile
            GameObject proj = Instantiate(projectilePrefab, data.origin, Quaternion.identity);
            Rigidbody2D rb2D = proj.GetComponent<Rigidbody2D>();
            rb2D.velocity = attackDir * speed;
        }
    }
}
