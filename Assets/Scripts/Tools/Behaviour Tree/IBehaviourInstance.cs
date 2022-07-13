using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public interface IBehaviourInstance
    {
        public abstract BehaviourTree GetBehaviourTree();
        public abstract BehaviourInstanceProperty GetInstanceProperty(string uniqueID);
        public abstract BehaviourInstanceProperty[] GetInstanceProperties();
        public abstract void SetInstanceProperties(BehaviourInstanceProperty[] props);
    }
}
