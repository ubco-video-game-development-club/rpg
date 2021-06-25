using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 66)]
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
    public class BehaviourTreeNode : ISerializationCallbackReceiver
    {
        public Dictionary<string, VariableProperty> Properties { get => properties; }
        public IBehaviourTreeNode Node { get => node; }
        private Dictionary<string, VariableProperty> properties = new Dictionary<string, VariableProperty>();
        private IBehaviourTreeNode node;
        [SerializeField] private PropertyInfo[] propertyInfo;
        [SerializeField] private string nodeTypeName;

        public BehaviourTreeNode(IBehaviourTreeNode node)
        {
            this.node = node;
        }

        public void AddProperty(string name, VariableProperty property)
        {
            if (properties.ContainsKey(name)) return;
            properties.Add(name, property);
        }

        public VariableProperty GetProperty(string name)
        {
            if (!properties.ContainsKey(name)) return null;
            return properties[name];
        }

        public bool RemoveProperty(string name) => properties.Remove(name);
        public NodeStatus Tick(Tree<BehaviourTreeNode>.Node self, Agent agent) => node.Tick(self, agent);

        public void OnBeforeSerialize()
        {
            if (properties == null) properties = new Dictionary<string, VariableProperty>();
            propertyInfo = new PropertyInfo[properties.Count];
            int i = 0;
            foreach (string name in properties.Keys)
            {
                propertyInfo[i].name = name;
                propertyInfo[i].property = properties[name];
                i++;
            }

            if (node == null) nodeTypeName = "";
            else nodeTypeName = node.GetType().FullName;
        }

        public void OnAfterDeserialize()
        {
            if (properties == null) properties = new Dictionary<string, VariableProperty>();
            foreach (PropertyInfo info in propertyInfo)
            {
                properties.Add(info.name, info.property);
            }

            System.Type type = System.Type.GetType(nodeTypeName);
            if (type == null) node = null;
            else node = (IBehaviourTreeNode)System.Activator.CreateInstance(type);
        }

        [System.Serializable]
        private struct PropertyInfo
        {
            public string name;
            public VariableProperty property;
        }
    }
}
