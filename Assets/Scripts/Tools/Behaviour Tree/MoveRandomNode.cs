using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class MoveRandomNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("origin", new VariableProperty(VariableProperty.Type.Vector));
            behaviour.SetProperty("max-distance", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("move-speed", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            // Get random target position
            Vector2 pos = agent.transform.position;
            if (!agent.HasProperty("random-pos"))
            {
                Vector2 origin = self.Element.GetProperty("origin").GetVector();
                float maxDist = (float)self.Element.GetProperty("max-distance").GetNumber();
                agent.SetProperty("random-pos", origin + Random.insideUnitCircle * maxDist);
            }

            // Move towards destination
            Vector2 dest = (Vector2)agent.GetProperty("random-pos");
            float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();
            if (Vector2.SqrMagnitude(dest - pos) > 0.1f)
            {
                agent.transform.position = Vector2.MoveTowards(pos, dest, moveSpeed * Time.deltaTime);
                return NodeStatus.Running;
            }

            agent.RemoveProperty("random-pos");
            return NodeStatus.Success;
        }
    }
}
