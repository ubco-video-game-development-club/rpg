using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack", order = 51)]
public class Attack : ScriptableObject
{
    [SerializeField] private float cooldown = 1f;
    public float Cooldown { get { return cooldown; } }

    [SerializeField] private int damage = 1;
    public int Damage { get { return damage; } }

    private bool enabled = true;
    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public void Invoke()
    {
        Debug.Log("Attack invoked");
    }
}
