using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_ID = "target-entity-id";
        private const string PROP_DEST = "index-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_ID, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC
            string targetID = behaviour.GetProperty(PROP_TARGET_ID).GetString();
            QuestGiver target = Entity.Find<QuestGiver>(targetID);

            // Get and save the dialogue index in destination prop
            string indexDest = behaviour.GetProperty(PROP_DEST).GetString();
            obj.SetProperty(indexDest, target.ActiveIndex);

            return NodeStatus.Success;
        }
    }
}