using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class GetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_NPC_INPUT = "npc-input";
        private const string PROP_INDEX_OUTPUT = "index-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_NPC_INPUT);
            behaviour.AddOutputProperty(PROP_INDEX_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC (or self if empty)
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_INPUT).GetString();
            GameObject npc = npcSrc == "" ? obj.gameObject : (GameObject)obj.GetProperty(npcSrc);

            // Get and save the dialogue index in destination prop
            string indexDest = behaviour.GetProperty(instance, PROP_INDEX_OUTPUT).GetString();
            obj.SetProperty(indexDest, npc.GetComponent<NPC>().ActiveIndex);

            return NodeStatus.Success;
        }
    }
}