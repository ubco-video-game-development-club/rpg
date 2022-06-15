using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class SubTreeNode : IBehaviourTreeNode
    {
        private const string PROP_SUBTREE = "subtree";
        
        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_SUBTREE, new VariableProperty(VariableProperty.Type.Object, typeof(BehaviourTree)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            BehaviourTree subtree = (BehaviourTree)behaviour.GetProperty(instance, PROP_SUBTREE).GetObject();
            NodeStatus status = subtree.Root.Element.Tick(self, obj, instance);
            return status;
        }
    }
}