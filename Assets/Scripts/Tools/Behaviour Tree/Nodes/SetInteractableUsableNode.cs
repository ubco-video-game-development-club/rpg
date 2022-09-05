using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetInteractableUsableNode : IBehaviourTreeNode
    {
        private const string PROP_INTERACTABLE_INPUT = "interactable-input";
        private const string PROP_USABLE = "usable";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_INTERACTABLE_INPUT);
            behaviour.AddProperty(PROP_USABLE, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get NPC
            string interactableSrc = behaviour.GetProperty(instance, PROP_INTERACTABLE_INPUT).GetString();
            GameObject interactable = obj.GetProperty(interactableSrc) as GameObject;

            // Set enemy status
            bool usable = behaviour.GetProperty(instance, PROP_USABLE).GetBoolean();
            interactable.GetComponent<Interactable>().SetUsable(usable);

            return NodeStatus.Success;
        }
    }
}
