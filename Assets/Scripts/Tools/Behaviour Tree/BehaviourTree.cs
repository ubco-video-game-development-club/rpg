using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 65)]
    public class BehaviourTree : ScriptableObject
    {
        public Tree<BehaviourTreeNode> tree;
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
        private Dictionary<string, VariableProperty> properties;

        public void AddProperty(VariableProperty property)
        {
            if(properties.ContainsKey(property.Name)) return;

            properties.Add(property.Name, property);
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
