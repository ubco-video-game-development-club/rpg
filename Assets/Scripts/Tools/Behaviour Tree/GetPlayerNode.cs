using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetPlayerNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Player player = null;
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            if (target == null || !target.TryGetComponent<Player>(out player))
            {
                return NodeStatus.Failure;
            }

            Vector2 playerPos = player.transform.position;
            agent.SetProperty("target", player);
            agent.SetProperty("destination", playerPos);
            return NodeStatus.Success;
        }
    }
}
