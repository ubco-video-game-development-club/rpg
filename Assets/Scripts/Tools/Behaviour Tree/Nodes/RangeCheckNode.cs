using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RangeCheckNode : IBehaviourTreeNode
    {
        private const string PROP_RANGE = "range";
        private const string PROP_POSITION_SRC = "position-source";

        public void Init(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_RANGE, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_POSITION_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Behaviour behaviour = self.Element;

            // Get the source position
            string src = behaviour.GetProperty(PROP_POSITION_SRC).GetString();
            if (agent.HasProperty(src))
            {
                // Get the distance between source and this agent
                Vector2 srcPosition = (Vector2)agent.GetProperty(src);
                float dist = Vector2.Distance(srcPosition, agent.transform.position);

                // Check whether we're in the specified range
                float range = (float)behaviour.GetProperty(PROP_RANGE).GetNumber();
                if (dist <= range)
                {
                    return NodeStatus.Success;
                }
            }
            return NodeStatus.Failure;
        }
    }
}