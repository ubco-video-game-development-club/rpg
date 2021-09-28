using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace RPG{

    public class BaseNPC: Actor
    {
        QuestGiver dialogue;
        Agent agent;

        // Start is called before the first frame update
        void Start()
        {
            dialogue=GetComponent<QuestGiver>();
            agent=GetComponent<Agent>();
            InitiateDialogue();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        //Force Dialogue with player;
        public void InitiateDialogue(){
            Player p=GameObject.FindGameObjectWithTag("player").GetComponent<Player>();
            dialogue.Interact(p);
            //disable the behaviour script in the meantime
            agent.DisableBehaviours();
            
            //TODO Detect when dialogue box is closed then switch to regular behaviours
            agent.EnableBehaviours();
        }
    }
}
