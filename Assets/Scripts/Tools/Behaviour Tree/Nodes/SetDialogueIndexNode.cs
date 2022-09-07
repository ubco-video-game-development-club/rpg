using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_INPUT = "npc-input";
        private const string PROP_INDEX = "dialogue-index";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_NPC_INPUT);
            behaviour.AddProperty(PROP_INDEX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC (or self if empty)
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_INPUT).GetString();
            GameObject npc = npcSrc == "" ? obj.gameObject : (GameObject)obj.GetProperty(npcSrc);

            // Set the dialogue index
            int index = (int)behaviour.GetProperty(instance, PROP_INDEX).GetNumber();
            npc.GetComponent<NPC>().ActiveIndex = index;

            return NodeStatus.Success;
        }
    }
}