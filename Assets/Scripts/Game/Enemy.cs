using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    void Start()
    {
        OnHealthChanged.AddListener(PrintHealth);
        OnDeath.AddListener(PrintDeath);
    }

    private void PrintHealth(int health)
    {
        Debug.Log(health);
    }

    private void PrintDeath()
    {
        Debug.Log("Died");
    }
}
