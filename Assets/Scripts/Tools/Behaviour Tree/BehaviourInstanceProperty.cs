using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [System.Serializable]
    public class BehaviourInstanceProperty
    {
        public string name;
        public VariableProperty value;

        public BehaviourInstanceProperty(string name, VariableProperty value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
