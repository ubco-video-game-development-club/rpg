using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class SuccessNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            return NodeStatus.Success;
        }
    }
}
