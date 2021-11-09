using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Agent : BehaviourObject
    {
        [SerializeField] private BehaviourTree behaviourTree;

        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }

        private Tree<Behaviour>.Node root;

        protected override void Awake()
        {
            base.Awake();
            root = behaviourTree.Root;
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected void Update()
        {
            root.Element.Tick(root, this);
        }
    }
}
