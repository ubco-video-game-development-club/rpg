using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree{
    public class ForceDialogueNode : IBehaviourTreeNode
    {   
        private const string MAX_INTERACT_DISTANCE="max-interact";
        private const string MIN_INTERACT_DISTANCE="min-interact";

        public void Serialize(Behaviour behaviour){
            behaviour.Properties.Add(MAX_INTERACT_DISTANCE,new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(MIN_INTERACT_DISTANCE,new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour=self.Element;
            QuestGiver dialogue=obj.GetComponent<QuestGiver>();
            Player p=GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //Checks if the player or the QuestGiver(DialogueScript later) is null
            if(dialogue!=null && p!=null){
                Vector2 currentPosition=obj.transform.position, targetPosition=p.transform.position;
                float distance=Vector2.Distance(currentPosition,targetPosition);
                float minDist=(float)behaviour.Properties[MIN_INTERACT_DISTANCE].GetNumber();
                float maxDist=(float)behaviour.Properties[MAX_INTERACT_DISTANCE].GetNumber();
                //Checks if the distance between the player and the Initiator fit within the min and max parameters
                if(distance>=minDist && distance<=maxDist){
                    dialogue.Interact(p);
                    return NodeStatus.Success;
                }

            }
            return NodeStatus.Failure;
        }
    
    }

}
