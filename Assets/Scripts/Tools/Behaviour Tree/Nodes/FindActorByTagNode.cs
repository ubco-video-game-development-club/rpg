using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours 
{
    public class FindActorByTagNode : IBehaviourTreeNode
    {
        private const string PROP_TAG_NAME = "tag-name";
        private const string PROP_ACTOR_OUTPUT = "actor-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_TAG_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.AddOutputProperty(PROP_ACTOR_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            
            string tag = behaviour.GetProperty(instance, PROP_TAG_NAME).GetString();
            GameObject actor = GameObject.FindWithTag(tag);
            if(actor == null) return NodeStatus.Failure;
            
            string destination = behaviour.GetProperty(instance, PROP_ACTOR_OUTPUT).GetString();
            obj.SetProperty(destination, actor);
            return NodeStatus.Success;
        }
    }
}