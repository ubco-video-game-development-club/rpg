using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace RPG{
    /*
    This Class is purely meant to manage until I migrate all of these into separate BehaviourNodes.
    I apologize :'( I just didn't have time to tamper with all of the engine yet to implement further changes.
    */
    public class NPCMovement
    {
        private float followDistance;
        private bool isRunning;
        public NPCMovement(float f=1f){
            followDistance=f;
            isRunning=false;
        }

        //Follow a player at a distance
        public void FollowPlayer(Vector2 position, Agent agent){
            Vector2 playerPos=(Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
            if(playerPos!=null){
                //Get the distance between two vectors
                float distance=Vector2.Distance(playerPos,position);
                if(distance>=followDistance){
                    //Set destination to where followDistance is on the line between the two.
                    float ratio=followDistance/distance;
                    agent.SetProperty("destination",Vector2.Lerp(playerPos,position,ratio));
                }

            }
            
        }

        public void FollowPath(Vector2 position, Agent agent){
            //TODO Follow a path via a list of Vector2s 
        }
    }
}

