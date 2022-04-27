using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface IBehaviourInstance
    {
        public abstract BehaviourTree GetBehaviourTree();
        public abstract BehaviourInstanceProperty GetInstanceProperty(string name);
        public abstract BehaviourInstanceProperty[] GetInstanceProperties();
        public abstract void SetInstanceProperties(BehaviourInstanceProperty[] props);
    }
}
