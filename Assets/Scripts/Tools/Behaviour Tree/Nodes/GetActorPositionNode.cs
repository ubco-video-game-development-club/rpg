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

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.Properties[PROP_ACTOR_SRC].GetString();
            if (obj.HasProperty(src))
            {
                string dest = behaviour.Properties[PROP_POSITION_DEST].GetString();
                GameObject actor = obj.GetProperty(src) as GameObject;
                Vector2 position = actor.transform.position;
                obj.SetProperty(dest, position);
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}