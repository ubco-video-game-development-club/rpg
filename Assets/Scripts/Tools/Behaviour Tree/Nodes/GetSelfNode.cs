using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetSelfNode : IBehaviourTreeNode
    {
        private const string PROP_OBJECT_DEST = "object-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_OBJECT_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            string dest = behaviour.GetProperty(PROP_OBJECT_DEST).GetString();
            obj.SetProperty(dest, obj.gameObject);
            return NodeStatus.Success;
        }
    }
}
