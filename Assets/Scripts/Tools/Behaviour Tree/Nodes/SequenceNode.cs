using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class SequenceNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            for (int i = 0; i < self.ChildCount; i++)
            {
                Tree<Behaviour>.Node child = self.GetChild(i);
                NodeStatus childStatus = child.Element.Tick(child, obj, instance);
                if (childStatus != NodeStatus.Success) return childStatus;
            }

            return NodeStatus.Success;
        }
    }
}
