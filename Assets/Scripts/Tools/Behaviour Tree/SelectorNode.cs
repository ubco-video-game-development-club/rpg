using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : BehaviourTreeNode
    {
        public override NodeStatus Tick(Tree<BehaviourTreeNode>.Node self)
        {
            for(int i = 0; i < self.ChildCount; i++)
            {
                Tree<BehaviourTreeNode>.Node child = self.GetChild(i);
                NodeStatus childStatus = child.Element.Tick(child);
                if(childStatus != NodeStatus.Failure) return childStatus;
            }

            return NodeStatus.Failure;
        }
    }
}
