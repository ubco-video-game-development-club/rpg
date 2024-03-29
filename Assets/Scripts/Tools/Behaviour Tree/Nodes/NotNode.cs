using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class NotNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Tree<Behaviour>.Node child = self.GetChild(0);
            NodeStatus childStatus = child.Element.Tick(child, obj, instance);
            return childStatus == NodeStatus.Failure ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
