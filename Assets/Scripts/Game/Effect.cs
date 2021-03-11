using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [SerializeField] protected Effect[] onHitActions;

    public abstract void Invoke();
}
