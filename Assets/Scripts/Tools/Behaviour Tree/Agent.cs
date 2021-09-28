using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private BehaviourTree behaviourTree;

        public Dictionary<string, object> Properties { get; private set; }

        private Tree<Behaviour>.Node root;

        private bool isEnabled;

        void Awake()
        {
            root = behaviourTree.Root;
            Properties = new Dictionary<string, object>();
            isEnabled=true;
        }

        void Update()
        {
            if(isEnabled){
                root.Element.Tick(root, this);
            }

        }

        public void SetProperty(string name, object property)
        {
            Properties[name] = property;
        }

        public object GetProperty(string name)
        {
            return HasProperty(name) ? Properties[name] : null;
        }

        public bool HasProperty(string name)
        {
            return Properties.ContainsKey(name);
        }

        public void RemoveProperty(string name)
        {
            Properties.Remove(name);
        }

        public void DisableBehaviours(){
            isEnabled=false;
        }

        public void EnableBehaviours(){
            isEnabled=true;
        }
    }
}
