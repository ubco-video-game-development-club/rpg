using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [System.Serializable]
    public class BehaviourInstanceProperty
    {
        public string name;
        public int index;
        public string nodeName;
        public VariableProperty value;

        public string UniqueID { get => name + index; }

        public BehaviourInstanceProperty(string name, int index, string nodeName, VariableProperty value)
        {
            this.name = name;
            this.index = index;
            this.nodeName = nodeName;
            this.value = value;
        }
    }
}
