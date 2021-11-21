using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    // TODO: turn this into HasFactionNode later!
    public class IsEnemyNode : IBehaviourTreeNode
    {
        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            return obj.GetComponent<NPC>().IsEnemy() ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
