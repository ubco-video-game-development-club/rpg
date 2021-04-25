using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class BehaviourTreeWindow : EditorWindow
{
    private const float INDENT_MULTIPLIER = 5.0f;
    private Vector2 CentreOfWindow { get { return this.position.size / 2.0f; } }
    private BehaviourTree.BehaviourTree selectedTree = null;
    private Vector2 scrollPos = Vector2.zero;

    void OnEnable()
    {
        UpdateSelectedTree();
    }

    void OnGUI()
    {
        if(selectedTree == null)
        {
            ShowMessage("Select a behaviour tree to begin editing.");
            return;
        }

        GUILayout.Space(5);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        Tree<BehaviourTreeNode> tree = selectedTree.tree;
        Tree<BehaviourTreeNode>.Node root = tree.Root ?? default(Tree<BehaviourTreeNode>.Node); //The default case should never happen
        ShowNode(root);
        GUILayout.EndScrollView();
    }

    void OnSelectionChange()
    {
        UpdateSelectedTree();
    }

    private void ShowNode(Tree<BehaviourTreeNode>.Node node, int indent = 0)
    {
        string name = node.Element.GetType().Name;
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Space(indent * INDENT_MULTIPLIER);
        GUILayout.Label(name);
       
        if(GUILayout.Button("+", GUILayout.Width(25)))
        {
            //TODO: Add child node
        }

        if(GUILayout.Button("-", GUILayout.Width(25)))
        {
            //TODO: Remove this node
        }
        
        GUILayout.EndHorizontal();

        for(int i = 0; i < node.ChildCount; i++)
        {
            var child = node.GetChild(i);
            ShowNode(child, indent + 1);
        }
    }

    private void UpdateSelectedTree()
    {
        if (Selection.activeObject is BehaviourTree.BehaviourTree)
        {
            selectedTree = Selection.activeObject as BehaviourTree.BehaviourTree;
            if(selectedTree.tree == null || selectedTree.tree.Root == null) selectedTree.tree = new Tree<BehaviourTreeNode>(new SequenceNode());
        }
        else selectedTree = null;

        Repaint();
    }

    private void ShowMessage(string message)
    {
        Vector2 position = CentreOfWindow;

        GUIContent content = new GUIContent(message);
        Vector2 size = GUI.skin.label.CalcSize(content);

        position.x -= size.x / 2.0f; //Centre the text
        GUI.Label(new Rect(position, size), message);
    }

    [MenuItem("Window/Behaviour Tree")]
    public static void CreateWindow()
    {
        BehaviourTreeWindow window = GetWindow<BehaviourTreeWindow>();
        window.titleContent = new GUIContent("Behaviour Tree");
    }
}
