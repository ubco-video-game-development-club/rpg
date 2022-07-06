using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class GetSelfNode : IBehaviourTreeNode
    {
        private const string PROP_OBJECT_OUTPUT = "object-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddOutputProperty(PROP_OBJECT_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            string dest = behaviour.GetProperty(instance, PROP_OBJECT_OUTPUT).GetString();
            obj.SetProperty(dest, obj.gameObject);
            return NodeStatus.Success;
        }
    }
}
