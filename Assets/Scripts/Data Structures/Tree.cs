using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tree<T>
{
    public Node Root { get; }

    public Tree(T root)
    {
        Root = new Node(root);
    }

    [System.Serializable]
    public struct Node
    {
        public int ChildCount { get => children.Count; }
        public T Element { get => element; }

        private T element;
        private List<Node> children;

        public Node(T element)
        {
            this.element = element;
            children = new List<Node>();
        }

        public void AddChild(Node child) => children.Add(child);
        public void RemoveChild(Node child) => children.Remove(child);
        public Node GetChild(int index) => children[index];
    }
}