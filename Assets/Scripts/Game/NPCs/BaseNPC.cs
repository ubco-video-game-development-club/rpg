using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace RPG{

    public class BaseNPC: Actor
    {
        private QuestGiver dialogue;
        private CircleCollider2D npcCollider;
        private Agent agent;

        private bool isNPCFollower;

        [SerializeField]private float interactionDistance=5.0f;

        private bool dialogueInitiated=false;//Delete this when we have a means of catching when dialogue is done

        // Start is called before the first frame update
        protected override void Awake() {
            base.Awake();
            dialogue=GetComponent<QuestGiver>();
            agent=GetComponent<Agent>();
            npcCollider=GetComponent<CircleCollider2D>();

        }

        // Update is called once per frame
        void Update()
        {
        }

        //Force Dialogue with player;
        public void InitiateDialogue(){
            GameObject player=GameObject.FindGameObjectWithTag("Player");
            //Detect if player is within range of the player
            if(!dialogueInitiated && player!=null && Vector2.Distance(transform.position, player.transform.position)<=interactionDistance){
                Debug.Log("Dialogue initiated");
                dialogue.Interact(player.GetComponent<Player>());
                dialogueInitiated=true;
                //disable the behaviour script in the meantime
                agent.DisableBehaviours();
            
                //TODO Detect when dialogue box is closed then switch to regular behaviours
                agent.EnableBehaviours();
            }
        }

        public void TurnIntoEnemy(){
            //Change to enemy Layer;
            gameObject.layer=LayerMask.NameToLayer("Enemy");

            //Doesnt if this doesn't have Enemy Script will add it
            if(gameObject.GetComponent<Enemy>()==null){
                gameObject.AddComponent<Enemy>();
            }
            else{
                gameObject.GetComponent<Enemy>().enabled=true;
            }
            //TODO add AttackBehaviour in BehaviourTree
        }

        public void TurnIntoCompanion(){
            //TODO Make Follower movement, interactable
            npcCollider.enabled=false;
        }
    }
}
