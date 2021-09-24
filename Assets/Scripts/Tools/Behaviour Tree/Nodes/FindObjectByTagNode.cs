using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree 
{
    public class FindObjectByTagNode : IBehaviourTreeNode
    {
        private const string PROP_ACTOR_DEST = "actor-destination";
        private const string PROP_TAG_NAME = "tag-name";

        private Behaviour behaviour;

        public void Init(Behaviour behaviour)
        {
            this.behaviour = behaviour;
            behaviour.Properties.Add(PROP_TAG_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_ACTOR_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            string tag = behaviour.Properties[PROP_TAG_NAME].GetString();
            GameObject actor = GameObject.FindWithTag(tag);
            if(actor == null) return NodeStatus.Failure;
            
            string destination = behaviour.Properties[PROP_ACTOR_DEST].GetString();
            agent.SetProperty(destination, actor);
            return NodeStatus.Success;
        }
    }
}