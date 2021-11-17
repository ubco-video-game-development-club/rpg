using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace RPG
{
    public class BaseNPC : Actor
    {
        private NPC dialogue;
        private CircleCollider2D npcCollider;

        private bool isNPCFollower;

        [SerializeField] private float interactionDistance = 5.0f;

        private bool dialogueInitiated = false; //Delete this when we have a means of catching when dialogue is done

        protected override void Awake()
        {
            base.Awake();
            dialogue = GetComponent<NPC>();
            npcCollider = GetComponent<CircleCollider2D>();
        }

        public void TurnIntoEnemy()
        {
            //Change to enemy Layer;
            gameObject.layer = LayerMask.NameToLayer("Enemy");

            //Doesnt if this doesn't have Enemy Script will add it
            if (gameObject.GetComponent<Enemy>() == null)
            {
                gameObject.AddComponent<Enemy>();
            }
            else
            {
                gameObject.GetComponent<Enemy>().enabled = true;
            }
            //TODO add AttackBehaviour in BehaviourTree
        }

        public void TurnIntoCompanion()
        {
            //TODO Make Follower movement, interactable
            npcCollider.enabled = false;
        }
    }
}
