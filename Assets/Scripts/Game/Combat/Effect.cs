using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class Effect : ScriptableObject
    {
        [SerializeField] protected Effect[] onHitActions;

        public abstract void Invoke(ActionData data);
    }
}
