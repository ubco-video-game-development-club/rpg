using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tree<T> : ISerializationCallbackReceiver
{
    public Node Root { get => root; }

    [SerializeField] private List<SerializableNode> serializedNodes;
    private Node root;

    public Tree(T root)
    {
        this.root = new Node(root);
    }

    public void OnBeforeSerialize()
    {
        if (serializedNodes == null) serializedNodes = new List<SerializableNode>();
        serializedNodes.Clear();
        SerializeNode(root);
    }

    public void OnAfterDeserialize()
    {
        if (serializedNodes.Count > 0) DeserializeNode(0, out root);
        else root = new Node(default(T));
    }

    private void SerializeNode(Node node)
    {
        SerializableNode snode = new SerializableNode(node.ChildCount, node.Element, serializedNodes.Count + 1);
        serializedNodes.Add(snode);
        for (int i = 0; i < node.ChildCount; i++)
        {
            SerializeNode(node.GetChild(i));
        }
    }

    private int DeserializeNode(int index, out Node node)
    {
        SerializableNode snode = serializedNodes[index];
        node = new Node(snode.element, snode.nodeIndex);
        for (int i = 0; i < snode.childCount; i++)
        {
            index = DeserializeNode(++index, out Node n);
            node.AddChild(n);
        }
        return index;
    }

    public struct Node
    {
        public int ChildCount { get => children == null ? 0 : children.Count; }
        public T Element { get => element; }
        public int NodeIndex { get => nodeIndex; }

        private T element;
        private List<Node> children;
        private int nodeIndex;

        public Node(T element, int nodeIndex = 0)
        {
            this.element = element;
            children = new List<Node>();
            this.nodeIndex = nodeIndex;
        }

        public void AddChild(Node child) => children.Add(child);
        public void RemoveChild(Node child) => children.Remove(child);
        public Node GetChild(int index) => children[index];
    }

    [System.Serializable]
    public struct SerializableNode
    {
        public int childCount;
        public T element;
        public int nodeIndex;

        public SerializableNode(int childCount, T element, int nodeIndex)
        {
            this.childCount = childCount;
            this.element = element;
            this.nodeIndex = nodeIndex;
        }
    }
}
