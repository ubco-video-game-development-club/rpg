using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    public class BehaviourTreeWindow : EditorWindow
    {
        private const float INDENT_MULTIPLIER = 10.0f;
        private Vector2 CentreOfWindow { get { return this.position.size / 2.0f; } }
        private BehaviourTree selectedTree = null;
        private Vector2 scrollPos = Vector2.zero;

        void OnEnable()
        {
            UpdateSelectedTree();
        }

        void OnGUI()
        {
            if (selectedTree == null)
            {
                ShowMessage("Select a behaviour tree to begin editing.");
                return;
            }

            GUILayout.Space(5);
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            Tree<BehaviourTreeNode> tree = selectedTree.tree;
            Tree<BehaviourTreeNode>.Node root = tree.Root;
            ShowNode(null, root);
            GUILayout.EndScrollView();
        }

        void OnSelectionChange()
        {
            UpdateSelectedTree();
        }

        private void ShowNode(Tree<BehaviourTreeNode>.Node? parent, Tree<BehaviourTreeNode>.Node node, int indent = 0)
        {
            string name = node.Element.Node.GetType().Name;
            Rect layout = EditorGUILayout.BeginHorizontal();
            layout.width -= 55;

            if (GUI.Button(layout, GUIContent.none, GUI.skin.box)) selectedTree.selectedNode = node.Element;

            GUILayout.Space((indent + 1) * INDENT_MULTIPLIER);
            GUILayout.Label(name);

            if (GUILayout.Button("+", GUILayout.Width(25))) AddChild(node);

            if (parent != null && GUILayout.Button("-", GUILayout.Width(25))) parent?.RemoveChild(node);

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                ShowNode(node, child, indent + 1);
            }
        }

        private void AddChild(Tree<BehaviourTreeNode>.Node parent)
        {
            GenericMenu menu = new GenericMenu();
            BehaviourTreeNodeType[] nodeTypes = (BehaviourTreeNodeType[])System.Enum.GetValues(typeof(BehaviourTreeNodeType));
            foreach (BehaviourTreeNodeType type in nodeTypes)
            {
                menu.AddItem(
                    new GUIContent(type.ToString()),
                    false,
                    () =>
                    {
                        IBehaviourTreeNode node = BehaviourTreeNodeCreator.Create(type);
                        BehaviourTreeNode bNode = new BehaviourTreeNode(node);
                        node.Init(bNode);
                        parent.AddChild(new Tree<BehaviourTreeNode>.Node(bNode));
                        EditorUtility.SetDirty(selectedTree);
                    }
                );
            }

            menu.ShowAsContext();
        }

        private void UpdateSelectedTree()
        {
            if (Selection.activeObject is BehaviourTree)
            {
                selectedTree = Selection.activeObject as BehaviourTree;
                if (selectedTree.tree == null || selectedTree.tree.Root.Element == null) 
                {
                    selectedTree.tree = new Tree<BehaviourTreeNode>(new BehaviourTreeNode(new SequenceNode()));
                }
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

}