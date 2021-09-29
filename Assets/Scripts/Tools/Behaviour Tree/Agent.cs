using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class Agent : BehaviourObject
    {
        [SerializeField] private BehaviourTree behaviourTree;

        private Tree<Behaviour>.Node root;

        protected override void Awake()
        {
            base.Awake();
            root = behaviourTree.Root;
        }

        protected void Update()
        {
            root.Element.Tick(root, this);
        }
    }
}
