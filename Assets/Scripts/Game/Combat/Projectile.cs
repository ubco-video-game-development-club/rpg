using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect;

        private ProjectileData data;
        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (MathUtils.IsLayerInMask(data.hitMask, col.gameObject.layer) && col.TryGetComponent<Actor>(out Actor target))
            {
                target.TakeDamage(data.damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                if (data.dieOnCollide)
                {
                    Destroy(gameObject);
                }
            }
        }

        public virtual void Fire(ProjectileData data)
        {
            this.data = data;
            rigidbody2D.velocity = data.direction.normalized * data.speed;
            float lifetime = data.range / data.speed;
            Destroy(gameObject, lifetime);
        }
    }
}
