using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class ForceDialogueNode : IBehaviourTreeNode
    {

        public void Serialize(Behaviour behaviour) { }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            NPC dialogue = obj.GetComponent<NPC>();
            Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //Checks if the player or the QuestGiver(DialogueScript later) is null
            if (dialogue != null && p != null)
            {
                dialogue.Interact(p);
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }

    }

}
