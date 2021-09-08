using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("move-range", new VariableProperty(VariableProperty.Type.Number));
            behaviour.SetProperty("move-speed", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            if (!agent.HasProperty("destination"))
            {
                return NodeStatus.Failure;
            }

            // Fail if we're out of range of the target
            Vector2 dest = (Vector2)agent.GetProperty("destination");
            Vector2 pos = agent.transform.position;
            float moveRange = (float)self.Element.GetProperty("move-range").GetNumber();
            if (Vector2.SqrMagnitude(dest - pos) > moveRange * moveRange)
            {
                ClearProperties(agent);
                return NodeStatus.Failure;
            }

            Debug.Log("Moving");

            // Move towards destination
            float moveSpeed = (float)self.Element.GetProperty("move-speed").GetNumber();
            if (Vector2.SqrMagnitude(dest - pos) > 0.1f)
            {
                agent.transform.position = Vector2.MoveTowards(pos, dest, moveSpeed * Time.deltaTime);
                return NodeStatus.Running;
            }

            ClearProperties(agent);
            return NodeStatus.Success;
        }

        private void ClearProperties(Agent agent)
        {
            agent.RemoveProperty("destination");
        }
    }
}
