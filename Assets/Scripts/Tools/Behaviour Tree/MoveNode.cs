using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("move-speed", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("move-timeout", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            if (!agent.HasProperty("destination"))
            {
                return NodeStatus.Failure;
            }

            Vector2 dest = (Vector2)agent.GetProperty("destination");
            Vector2 pos = agent.transform.position;
            float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();

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
                        agent.SetProperty("move-timeout-time", 0f);
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

            return NodeStatus.Success;
        }
    }
}
