using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 65)]
    public class BehaviourTree : ScriptableObject
    {
        public Tree<BehaviourTreeNode> tree;
        [System.NonSerialized] public BehaviourTreeNode selectedNode;
    }

    public enum NodeStatus
    {
        Running,
        Success,
        Failure
    }

    [System.Serializable]
    public abstract class BehaviourTreeNode
    {
        public Dictionary<string, VariableProperty> Properties { get => properties; }
        private Dictionary<string, VariableProperty> properties = new Dictionary<string, VariableProperty>();

        public void AddProperty(string name, VariableProperty property)
        {
            if(properties.ContainsKey(name)) return;
            properties.Add(name, property);
        }

        public VariableProperty GetProperty(string name)
        {
            if(!properties.ContainsKey(name)) return null;
            return properties[name];
        }

        public bool RemoveProperty(string name) => properties.Remove(name);
        public abstract NodeStatus Tick(Tree<BehaviourTreeNode>.Node self);
    }
}
