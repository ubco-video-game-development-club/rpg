using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public abstract class Condition
    {
        public abstract void Initialize();
        
        public abstract bool Evaluate();
    }
}
