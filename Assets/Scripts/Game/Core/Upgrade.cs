using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class Upgrade : ScriptableObject
    {
        [SerializeField] private EntityProperty[] stats;
        public EntityProperty[] Stats { get => stats; }

        public virtual void ApplyTo(Entity entity)
        {
            foreach (EntityProperty stat in stats)
            {
                stat.ApplyTo(entity);
            }
        }

        public virtual void RemoveFrom(Entity entity)
        {
            foreach (EntityProperty stat in stats)
            {
                stat.RemoveFrom(entity);
            }
        }
    }
}
