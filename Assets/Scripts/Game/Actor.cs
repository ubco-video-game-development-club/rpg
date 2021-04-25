using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : Entity
{
    [SerializeField] private int maxHealth;

    public int Health
    {
        get { return GetProperty<int>(PropertyName.Health); }
        private set { SetProperty<int>(PropertyName.Health, value); }
    }

    private UnityEvent<int> onHealthChanged = new UnityEvent<int>();
    public UnityEvent<int> OnHealthChanged { get { return onHealthChanged; } }

    private UnityEvent onDeath = new UnityEvent();
    public UnityEvent OnDeath { get { return onDeath; } }

    protected virtual void Awake()
    {
        Health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
        onHealthChanged.Invoke(Health);
        if (Health <= 0) Die();
    }

    private void Die()
    {
        onDeath.Invoke();
        Destroy(gameObject);
    }
}
