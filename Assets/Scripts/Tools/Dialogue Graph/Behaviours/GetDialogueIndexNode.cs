using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_SRC = "npc-source";
        private const string PROP_DEST = "index-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NPC_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC (or self if empty)
            string npcSrc = behaviour.GetProperty(PROP_NPC_SRC).GetString();
            GameObject npc = npcSrc == "" ? obj.gameObject : (GameObject)obj.GetProperty(npcSrc);

            // Get and save the dialogue index in destination prop
            string indexDest = behaviour.GetProperty(PROP_DEST).GetString();
            obj.SetProperty(indexDest, npc.GetComponent<QuestGiver>().ActiveIndex);

            return NodeStatus.Success;
        }
    }
}