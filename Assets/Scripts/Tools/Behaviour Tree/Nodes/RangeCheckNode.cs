using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class RangeCheckNode : IBehaviourTreeNode
    {
        private const string PROP_POSITION_INPUT = "position-input";
        private const string PROP_RANGE = "range";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_POSITION_INPUT);
            behaviour.AddProperty(PROP_RANGE, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get the source position
            string src = behaviour.GetProperty(instance, PROP_POSITION_INPUT).GetString();
            if (obj.HasProperty(src))
            {
                // Get the distance between source and this agent
                Vector2 srcPosition = (Vector2)obj.GetProperty(src);
                float dist = Vector2.Distance(srcPosition, obj.transform.position);

                // Check whether we're in the specified range
                float range = (float)behaviour.GetProperty(instance, PROP_RANGE).GetNumber();
                if (dist <= range)
                {
                    return NodeStatus.Success;
                }
            }
            return NodeStatus.Failure;
        }
    }
}