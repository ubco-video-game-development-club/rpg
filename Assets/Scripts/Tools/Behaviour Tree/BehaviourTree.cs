using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 66)]
    public class BehaviourTree : ScriptableObject
    {
        public Tree<Behaviour> tree;
        public Tree<Behaviour>.Node Root { get => tree.Root; }
        [System.NonSerialized] public Behaviour selectedNode;

        public bool RefreshInstance(IBehaviourInstance instance)
        {
            // Gather the list of instanced props from the tree
            List<BehaviourInstanceProperty> treeProps = new List<BehaviourInstanceProperty>();
            Queue<Tree<Behaviour>.Node> nodes = new Queue<Tree<Behaviour>.Node>();
            nodes.Enqueue(Root);
            while (nodes.Count > 0)
            {
                Tree<Behaviour>.Node node = nodes.Dequeue();

                foreach (string propName in node.Element.Properties.Keys)
                {
                    VariableProperty nodeProp = node.Element.GetProperty(null, propName);
                    if (nodeProp.Instanced)
                    {
                        string rawNodeName = node.Element.Node.ToString();
                        string nodeName = rawNodeName.Substring(rawNodeName.IndexOf(".") + 1);
                        treeProps.Add(new BehaviourInstanceProperty(propName, node.NodeIndex, nodeName, nodeProp));
                    }
                }

                for (int i = 0; i < node.ChildCount; i++)
                {
                    nodes.Enqueue(node.GetChild(i));
                }
            }

            // Refresh the properties on the instance
            bool changed = instance.GetInstanceProperties().Length > treeProps.Count;
            BehaviourInstanceProperty[] newProps = new BehaviourInstanceProperty[treeProps.Count];
            for (int i = 0; i < newProps.Length; i++)
            {
                BehaviourInstanceProperty treeProp = treeProps[i];
                BehaviourInstanceProperty currProp = instance.GetInstanceProperty(treeProp.UniqueID);

                newProps[i] = new BehaviourInstanceProperty(treeProp.name, treeProp.index, treeProp.nodeName, treeProp.value);
                if (currProp != null)
                {
                    newProps[i].value = currProp.value;
                    continue;
                }

                changed = true;
            }
            instance.SetInstanceProperties(newProps);
            return changed;
        }
    }

    public enum NodeStatus
    {
        Running,
        Success,
        Failure
    }

    [System.Serializable]
    public class Behaviour : ITreeNodeElement, ISerializationCallbackReceiver
    {
        [SerializeField] private PropertyInfo[] propertyInfo;
        [SerializeField] private string nodeTypeName;

        public int NodeIndex { get; private set; }

        public Dictionary<string, VariableProperty> Properties { get => properties; }
        private Dictionary<string, VariableProperty> properties = new Dictionary<string, VariableProperty>();

        public IBehaviourTreeNode Node { get => node; }
        private IBehaviourTreeNode node;

        public Behaviour(IBehaviourTreeNode node)
        {
            this.node = node;
        }

        public void AddInputProperty(string name) => AddDisplayProperty(name, VariableProperty.Display.Input);

        public void AddOutputProperty(string name) => AddDisplayProperty(name, VariableProperty.Display.Output);

        private void AddDisplayProperty(string name, VariableProperty.Display displayType)
        {
            VariableProperty prop = new VariableProperty(VariableProperty.Type.String);
            prop.DisplayType = displayType;
            prop.Set(name.Substring(0, name.LastIndexOf("-")));
            AddProperty(name, prop);
        }

        public void AddProperty(string name, VariableProperty property)
        {
            properties[name] = property;
        }

        public bool HasProperty(string name)
        {
            return properties.ContainsKey(name);
        }

        public VariableProperty GetProperty(IBehaviourInstance instance, string name)
        {
            if (instance != null)
            {
                BehaviourInstanceProperty instanceProp = instance.GetInstanceProperty(name + NodeIndex);
                if (instanceProp != null)
                {
                    return instanceProp.value;
                }
            }
            return properties.ContainsKey(name) ? properties[name] : null;
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance) => node.Tick(self, obj, instance);

        public void SetNodeIndex(int nodeIndex)
        {
            NodeIndex = nodeIndex;
        }

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
