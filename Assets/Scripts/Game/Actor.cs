using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : Entity
{
    private UnityEvent<int> onHealthChanged = new UnityEvent<int>();
    private UnityEvent onDeath = new UnityEvent();

    public int Health
    {
        get { return GetProperty<int>(PropertyName.HEALTH); }
        private set { SetProperty<int>(PropertyName.HEALTH, value); }
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
    }
}
