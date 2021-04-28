using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface IBehaviourTreeNode
    {
        void Init(BehaviourTreeNode self);
        NodeStatus Tick(Tree<BehaviourTreeNode>.Node self);
    }
}
