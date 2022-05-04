using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class IdleNode : IBehaviourTreeNode
    {
        private const string PROP_IDLE_DURATION = "idle-duration";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_IDLE_DURATION, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get idle time
            double idleTime = 0;
            if (obj.HasProperty("idle-time"))
            {
                idleTime = (double)obj.GetProperty("idle-time");
            }

            // Check if we've been idling for long enough
            double idleDuration = behaviour.GetProperty(instance, PROP_IDLE_DURATION).GetNumber();
            if (idleTime < idleDuration)
            {
                obj.SetProperty("idle-time", idleTime + Time.deltaTime);
                return NodeStatus.Running;
            }

            // Clear idle timer
            obj.RemoveProperty("idle-time");
            return NodeStatus.Success;
        }
    }
}
