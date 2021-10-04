using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface IBehaviourTreeNode
    {
        void Serialize(Behaviour behaviour);
        NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj);
    }
}
