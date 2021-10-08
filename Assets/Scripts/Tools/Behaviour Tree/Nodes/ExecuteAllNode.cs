using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ExecuteAllNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            NodeStatus status = NodeStatus.Success;
            for (int i = 0; i < self.ChildCount; i++)
            {
                Tree<Behaviour>.Node child = self.GetChild(i);
                NodeStatus childStatus = child.Element.Tick(child, obj);
                if (childStatus == NodeStatus.Running) status = NodeStatus.Running;
            }
            return status;
        }
    }
}
