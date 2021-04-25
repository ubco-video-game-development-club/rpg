using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviourTreeWindow : EditorWindow
{
    private Vector2 CentreOfWindow { get { return this.position.size / 2.0f; } }
    private BehaviourTree.BehaviourTree selectedTree = null;

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
    }

    void OnSelectionChange()
    {
        UpdateSelectedTree();
    }

    private void UpdateSelectedTree()
    {
        if (Selection.activeObject is BehaviourTree.BehaviourTree) selectedTree = Selection.activeObject as BehaviourTree.BehaviourTree;
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
