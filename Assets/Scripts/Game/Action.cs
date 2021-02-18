using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    [SerializeField] protected Action[] onHitActions;

    public abstract void Invoke();
}
