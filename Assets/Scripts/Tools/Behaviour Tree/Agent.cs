using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private BehaviourTree behaviourTree;
        private Tree<BehaviourTreeNode>.Node root;

        void Awake()
        {
            root = behaviourTree.tree.Root;
        }

        void Update()
        {
            root.Element.Tick(root, this);
        }
    }
}
