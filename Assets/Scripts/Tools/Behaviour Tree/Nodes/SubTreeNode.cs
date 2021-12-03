using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SubTreeNode : IBehaviourTreeNode
    {
        private const string PROP_SUBTREE = "subtree";
        
        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_SUBTREE, new VariableProperty(VariableProperty.Type.Object, typeof(BehaviourTree)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            BehaviourTree subtree = (BehaviourTree)self.Element.GetProperty(PROP_SUBTREE).GetObject();
            NodeStatus status = subtree.Root.Element.Tick(self, obj);
            return status;
        }
    }
}