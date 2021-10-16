using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class HasTraitNode : IBehaviourTreeNode
    {
        private const string PROP_ACTOR_SOURCE = "actor-source";
        private const string PROP_TRAIT = "trait";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_ACTOR_SOURCE, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_TRAIT, new VariableProperty(VariableProperty.Type.Enum, typeof(Trait)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target actor
            string actorSrc = behaviour.GetProperty(PROP_ACTOR_SOURCE).GetString();
            Actor target = actorSrc == "" ? obj.GetComponent<Actor>() : (Actor)obj.GetProperty(actorSrc);

            // Return whether the target has the given trait
            Trait trait = behaviour.GetProperty(PROP_TRAIT).GetEnum<Trait>();
            return target.HasTrait(trait) ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
