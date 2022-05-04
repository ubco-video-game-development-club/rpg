using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours 
{
    public class FindActorByTagNode : IBehaviourTreeNode
    {
        private const string PROP_ACTOR_DEST = "actor-destination";
        private const string PROP_TAG_NAME = "tag-name";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TAG_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_ACTOR_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            
            string tag = behaviour.GetProperty(instance, PROP_TAG_NAME).GetString();
            GameObject actor = GameObject.FindWithTag(tag);
            if(actor == null) return NodeStatus.Failure;
            
            string destination = behaviour.GetProperty(instance, PROP_ACTOR_DEST).GetString();
            obj.SetProperty(destination, actor);
            return NodeStatus.Success;
        }
    }
}