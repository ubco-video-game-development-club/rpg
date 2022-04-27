using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent : BehaviourObject, IBehaviourInstance
    {
        [SerializeField] private BehaviourTree behaviourTree;

        [SerializeField][HideInInspector] private BehaviourInstanceProperty[] instanceProperties;

        private Tree<Behaviour>.Node root;

        public Rigidbody2D Rigidbody2D { get; private set; }

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

        public BehaviourTree GetBehaviourTree()
        {
            return behaviourTree;
        }

        public BehaviourInstanceProperty GetInstanceProperty(string name)
        {
            foreach (BehaviourInstanceProperty instanceProperty in instanceProperties)
            {
                if (instanceProperty.name == name)
                {
                    return instanceProperty;
                }
            }
            return null;
        }

        public BehaviourInstanceProperty[] GetInstanceProperties()
        {
            return instanceProperties;
        }

        public void SetInstanceProperties(BehaviourInstanceProperty[] props)
        {
            instanceProperties = props;
        }
    }
}
