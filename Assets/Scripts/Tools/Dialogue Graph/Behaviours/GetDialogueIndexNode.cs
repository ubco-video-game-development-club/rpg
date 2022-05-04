using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
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

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC (or self if empty)
            string npcSrc = behaviour.GetProperty(instance, PROP_NPC_SRC).GetString();
            GameObject npc = npcSrc == "" ? obj.gameObject : (GameObject)obj.GetProperty(npcSrc);

            // Get and save the dialogue index in destination prop
            string indexDest = behaviour.GetProperty(instance, PROP_DEST).GetString();
            obj.SetProperty(indexDest, npc.GetComponent<NPC>().ActiveIndex);

            return NodeStatus.Success;
        }
    }
}