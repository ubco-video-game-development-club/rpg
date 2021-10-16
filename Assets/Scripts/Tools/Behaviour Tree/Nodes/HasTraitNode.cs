using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class HasTraitNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_ID = "target-actor-id";
        private const string PROP_TRAIT = "trait";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_ID, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_TRAIT, new VariableProperty(VariableProperty.Type.Enum, typeof(Trait)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target actor
            string targetID = behaviour.GetProperty(PROP_TARGET_ID).GetString();
            Actor target = Entity.Find<Actor>(targetID);
            if (target == null)
            {
                Debug.LogError("Failed to find Actor with ID: " + targetID);
            }

            // Return whether the target has the given trait
            Trait trait = behaviour.GetProperty(PROP_TRAIT).GetEnum<Trait>();
            return target.HasTrait(trait) ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}
