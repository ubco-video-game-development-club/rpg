using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : IBehaviourTreeNode
    {
        public void Init(BehaviourTreeNode self) { }

        public NodeStatus Tick(Tree<BehaviourTreeNode>.Node self, Agent agent)
        {
            for (int i = 0; i < self.ChildCount; i++)
            {
                Tree<BehaviourTreeNode>.Node child = self.GetChild(i);
                NodeStatus childStatus = child.Element.Tick(child, agent);
                if (childStatus != NodeStatus.Failure) return childStatus;
            }

            return NodeStatus.Failure;
        }
    }
}
