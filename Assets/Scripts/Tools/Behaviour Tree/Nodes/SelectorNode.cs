using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class SelectorNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            for (int i = 0; i < self.ChildCount; i++)
            {
                Tree<Behaviour>.Node child = self.GetChild(i);
                NodeStatus childStatus = child.Element.Tick(child, obj);
                if (childStatus != NodeStatus.Failure) return childStatus;
            }

            return NodeStatus.Failure;
        }
    }
}
