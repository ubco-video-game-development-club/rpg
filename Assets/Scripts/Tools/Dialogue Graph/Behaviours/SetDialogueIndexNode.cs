using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class SetDialogueIndexNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_ID = "target-entity-id";
        private const string PROP_INDEX = "dialogue-index";

        public void Init(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_ID, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_INDEX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // Get the target NPC
            string targetID = behaviour.GetProperty(PROP_TARGET_ID).GetString();
            QuestGiver target = Entity.Find(targetID).GetComponent<QuestGiver>();

            // Set the dialogue index
            int index = (int)behaviour.GetProperty(PROP_INDEX).GetNumber();
            target.ActiveIndex = index;

            return NodeStatus.Success;
        }
    }
}