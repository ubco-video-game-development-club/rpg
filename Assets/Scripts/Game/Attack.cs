using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack", order = 60)]
public class Attack : ScriptableObject
{
    [SerializeField] private Action[] actions;

    [SerializeField] private float cooldown = 1f;
    public float Cooldown { get { return cooldown; } }

    private bool enabled = true;
    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public void Invoke()
    {
        Debug.Log("Attack invoked");
        foreach (Action action in actions)
        {
            action.Invoke();
        }
    }
}
