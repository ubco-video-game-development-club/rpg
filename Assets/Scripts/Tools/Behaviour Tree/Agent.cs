using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class Agent : BehaviourObject
    {
        [SerializeField] private BehaviourTree behaviourTree;
        private bool isEnabled;
        private Tree<Behaviour>.Node root;

        protected override void Awake()
        {
            base.Awake();
            root = behaviourTree.Root;
            isEnabled=true;
        }

        protected void Update()
        {
            if(isEnabled){
                root.Element.Tick(root, this);
            }

        }

        public void DisableBehaviours(){
            isEnabled=false;
        }

        public void EnableBehaviours(){
            isEnabled=true;
        }
    }
}
