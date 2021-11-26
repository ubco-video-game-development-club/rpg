using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class SetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_SRC = "npc-source";
        private const string PROP_INDEX = "dialogue-index";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NPC_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_INDEX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC (or self if empty)
            string npcSrc = behaviour.GetProperty(PROP_NPC_SRC).GetString();
            GameObject npc = npcSrc == "" ? obj.gameObject : (GameObject)obj.GetProperty(npcSrc);

            // Set the dialogue index
            int index = (int)behaviour.GetProperty(PROP_INDEX).GetNumber();
            npc.GetComponent<QuestGiver>().ActiveIndex = index;

            return NodeStatus.Success;
        }
    }
}