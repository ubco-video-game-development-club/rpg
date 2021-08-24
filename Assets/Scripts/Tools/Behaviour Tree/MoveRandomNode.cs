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
            behaviour.SetProperty("move-timeout", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Debug.Log("We here");

            // Get random target position
            if (!agent.HasProperty("random-pos"))
            {
                Vector2 origin = self.Element.GetProperty("origin").GetVector();
                float maxDist = (float)self.Element.GetProperty("max-distance").GetNumber();
                agent.SetProperty("random-pos", origin + Random.insideUnitCircle * maxDist);
            }

            Vector2 dest = (Vector2)agent.GetProperty("random-pos");
            float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();
            Vector2 pos = agent.transform.position;

            // Timeout if agent is unable to move
            if (agent.HasProperty("prev-position"))
            {
                Vector2 prevPos = (Vector2)agent.GetProperty("prev-position");
                bool isImpeded = Vector2.SqrMagnitude(pos - prevPos) < moveSpeed * Time.deltaTime;
                if (agent.HasProperty("move-timeout-time") && isImpeded)
                {
                    float moveTimeoutTime = (float)agent.GetProperty("move-timeout-time") + Time.deltaTime;
                    agent.SetProperty("move-timeout-time", moveTimeoutTime);
                    if (moveTimeoutTime > (float)self.Element.GetProperty("move-timeout").GetNumber())
                    {
                        agent.RemoveProperty("random-pos");
                        agent.RemoveProperty("prev-position");
                        agent.RemoveProperty("move-timeout-time");
                        return NodeStatus.Failure;
                    }
                }
                else
                {
                    agent.SetProperty("move-timeout-time", 0f);
                }
            }
            agent.SetProperty("prev-position", pos);

            // Move towards destination
            if (Vector2.SqrMagnitude(dest - pos) > 0.1f)
            {
                agent.transform.position = Vector2.MoveTowards(pos, dest, moveSpeed * Time.deltaTime);
                return NodeStatus.Running;
            }

            agent.RemoveProperty("random-pos");
            agent.RemoveProperty("prev-position");
            agent.RemoveProperty("move-timeout-time");
            return NodeStatus.Success;
        }
    }
}
