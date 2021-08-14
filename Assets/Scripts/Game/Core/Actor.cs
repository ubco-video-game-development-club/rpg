using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public class Actor : Entity
    {
        [SerializeField] private int initialMaxHealth;

        public int MaxHealth
        {
            get => GetProperty<int>(PropertyName.MaxHealth);
            private set => SetProperty<int>(PropertyName.MaxHealth, value);
        }

        public int Health
        {
            get => GetProperty<int>(PropertyName.Health);
            private set => SetProperty<int>(PropertyName.Health, value);
        }

        private UnityEvent<int> onDamageTaken = new UnityEvent<int>();
        public UnityEvent<int> OnDamageTaken { get => onDamageTaken; }

        private UnityEvent onDeath = new UnityEvent();
        public UnityEvent OnDeath { get => onDeath; }

        protected virtual void Awake()
        {
            AddPropertyChangedListener<int>(PropertyName.MaxHealth, (maxHealth) => Health = maxHealth);

            MaxHealth = initialMaxHealth;
        }

        public void TakeDamage(int damage)
        {
            Health = Mathf.Max(0, Health - damage);
            onDamageTaken.Invoke(damage);
            if (Health <= 0) Die();
        }

        private void Die()
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
