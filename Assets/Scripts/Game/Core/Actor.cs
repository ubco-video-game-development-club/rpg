using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : Entity
{
    [SerializeField] private int initialMaxHealth;

    public int MaxHealth
    {
        get { return GetProperty<int>(PropertyName.MaxHealth); }
        private set { SetProperty<int>(PropertyName.MaxHealth, value); }
    }

    public int Health
    {
        get { return GetProperty<int>(PropertyName.Health); }
        private set { SetProperty<int>(PropertyName.Health, value); }
    }

    private UnityEvent<int> onDamageTaken = new UnityEvent<int>();
    public UnityEvent<int> OnDamageTaken { get { return onDamageTaken; } }

    private UnityEvent onDeath = new UnityEvent();
    public UnityEvent OnDeath { get { return onDeath; } }

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