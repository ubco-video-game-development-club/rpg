using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New ProjectileAttack", menuName = "Actions/Projectile Attack", order = 52)]
    public class ProjectileAttack : Action
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private int damage;
        [SerializeField] private bool passThroughTargets;

        public override void Invoke(ActionData data)
        {
            // Calculate direction based on mouse position
            Vector2 attackDir = (data.target - data.origin).normalized;

            // Create projectile
            Projectile proj = Instantiate(projectilePrefab, data.origin, Quaternion.identity);
            ProjectileData pData = new ProjectileData();
            pData.hitMask = data.targetMask;
            pData.speed = speed;
            pData.direction = attackDir;
            pData.range = range;
            pData.damage = damage;
            pData.dieOnCollide = !passThroughTargets;
            proj.Fire(pData);
        }
    }
}
