using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class NotNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Tree<Behaviour>.Node child = self.GetChild(0);
            NodeStatus childStatus = child.Element.Tick(child, obj);
            return childStatus == NodeStatus.Failure ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
