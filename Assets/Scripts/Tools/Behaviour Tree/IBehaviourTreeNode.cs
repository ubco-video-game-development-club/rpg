using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface IBehaviourTreeNode
    {
        void Init(Behaviour behaviour);
        NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent);
    }
}
