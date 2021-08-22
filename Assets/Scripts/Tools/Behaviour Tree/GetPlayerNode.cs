using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetPlayerNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("check-radius", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Player player = null;
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            if (target == null || !target.TryGetComponent<Player>(out player))
            {
                return NodeStatus.Failure;
            }

            Vector2 playerPos = player.transform.position;
            Vector2 selfPos = agent.transform.position;
            float checkRadius = (float)self.Element.GetProperty("check-radius").GetNumber();
            if (Vector2.SqrMagnitude(playerPos - selfPos) < checkRadius * checkRadius)
            {
                agent.SetProperty("target", player);
                agent.SetProperty("destination", playerPos);
                return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    }
}
