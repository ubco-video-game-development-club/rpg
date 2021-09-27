using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GetActorPositionNode : IBehaviourTreeNode
    {
        private const string PROP_ACTOR_SRC = "actor-source";
        private const string PROP_POSITION_DEST = "position-destination";

        public void Init(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_ACTOR_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_POSITION_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.Properties[PROP_ACTOR_SRC].GetString();
            if (agent.HasProperty(src))
            {
                string dest = behaviour.Properties[PROP_POSITION_DEST].GetString();
                GameObject actor = agent.GetProperty(src) as GameObject;
                Vector2 position = actor.transform.position;
                agent.SetProperty(dest, position);
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}