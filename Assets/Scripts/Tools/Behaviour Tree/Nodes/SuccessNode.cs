using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SuccessNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            return NodeStatus.Success;
        }
    }
}
