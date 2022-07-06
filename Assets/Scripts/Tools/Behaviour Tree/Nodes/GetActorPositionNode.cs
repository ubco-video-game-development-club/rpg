using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class GetActorPositionNode : IBehaviourTreeNode
    {
        private const string PROP_ACTOR_INPUT = "actor-input";
        private const string PROP_POSITION_OUTPUT = "position-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_ACTOR_INPUT);
            behaviour.AddOutputProperty(PROP_POSITION_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.GetProperty(instance, PROP_ACTOR_INPUT).GetString();
            if (obj.HasProperty(src))
            {
                string dest = behaviour.GetProperty(instance, PROP_POSITION_OUTPUT).GetString();
                GameObject actor = obj.GetProperty(src) as GameObject;
                Vector2 position = actor.transform.position;
                obj.SetProperty(dest, position);
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}