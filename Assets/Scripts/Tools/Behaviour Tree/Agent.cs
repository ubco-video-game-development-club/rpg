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
        public BehaviourTree BehaviourTree
        {
            get => behaviourTree;
            set => behaviourTree = value;
        }

        public Rigidbody2D Rigidbody2D { get; private set; }

        private Tree<Behaviour>.Node root;

        protected override void Awake()
        {
            base.Awake();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            root = behaviourTree.Root;
        }

        protected void Update()
        {
            root.Element.Tick(root, this);
        }
    }
}
