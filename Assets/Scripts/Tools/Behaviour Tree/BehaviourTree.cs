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
        public int PropertyCount { get => properties.Count; }
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

        public VariableProperty[] GetProperties()
        {
            VariableProperty[] buffer = new VariableProperty[PropertyCount];
            GetProperties(buffer, 0);
            return buffer;
        }

        public void GetProperties(VariableProperty[] buffer, int index) => properties.Values.CopyTo(buffer, index);
        public bool RemoveProperty(string name) => properties.Remove(name);
        public abstract NodeStatus Tick(Tree<BehaviourTreeNode>.Node self);
    }
}
