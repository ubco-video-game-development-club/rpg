using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent : BehaviourObject
    {
        [SerializeField] private BehaviourTree behaviourTree;

        public Rigidbody2D Rigidbody2D { get; private set; }

        private Tree<Behaviour>.Node root;
        private bool isEnabled;

        protected override void Awake()
        {
            base.Awake();

            Rigidbody2D = GetComponent<Rigidbody2D>();

            root = behaviourTree.Root;
            isEnabled = true;
        }

        protected void Update()
        {
            if (isEnabled)
            {
                root.Element.Tick(root, this);
            }
        }

        public void DisableBehaviours()
        {
            isEnabled = false;
        }

        public void EnableBehaviours()
        {
            isEnabled = true;
        }
    }
}
