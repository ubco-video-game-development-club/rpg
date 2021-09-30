using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 66)]
    public class BehaviourTree : ScriptableObject
    {
        public Tree<Behaviour> tree;
        public Tree<Behaviour>.Node Root { get => tree.Root; }
        [System.NonSerialized] public Behaviour selectedNode;
    }

    public enum NodeStatus
    {
        Running,
        Success,
        Failure
    }

    [System.Serializable]
    public class Behaviour : ISerializationCallbackReceiver
    {
        [SerializeField] private PropertyInfo[] propertyInfo;
        [SerializeField] private string nodeTypeName;

        public Dictionary<string, VariableProperty> Properties { get => properties; }
        private Dictionary<string, VariableProperty> properties = new Dictionary<string, VariableProperty>();

        public IBehaviourTreeNode Node { get => node; }
        private IBehaviourTreeNode node;

        public Behaviour(IBehaviourTreeNode node)
        {
            this.node = node;
        }

        public void SetProperty(string name, VariableProperty property)
        {
            properties[name] = property;
        }

        public VariableProperty GetProperty(string name)
        {
            return properties.ContainsKey(name) ? properties[name] : null;
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj) => node.Tick(self, obj);

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
