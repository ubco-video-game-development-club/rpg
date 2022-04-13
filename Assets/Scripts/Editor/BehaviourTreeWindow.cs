using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    public class BehaviourTreeWindow : EditorWindow
    {
        private const float INDENT_MULTIPLIER = 20.0f;
        private const float LINE_GAP = 2.0f;

        private Vector2 CentreOfWindow { get => this.position.size / 2.0f; }
        private BehaviourTree selectedTree = null;
        private Vector2 scrollPos = Vector2.zero;
        private Tree<Behaviour>.Node? dragNode = null;
        private Tree<Behaviour>.Node? dragParent = null;
        private Tree<Behaviour>.Node? hoverNode = null;
        private Vector2 mousePos;
        private HashSet<Tree<Behaviour>.Node> collapsedNodes = new HashSet<Tree<Behaviour>.Node>();

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
            Tree<Behaviour>.Node root = selectedTree.Root;
            hoverNode = null;
            ShowNode(null, root);
            GUILayout.EndScrollView();

            ProcessEvents(Event.current);
        }

        void OnSelectionChange()
        {
            UpdateSelectedTree();
        }

        private void ProcessEvents(Event e)
        {
            mousePos = e.mousePosition;
            if (dragNode != null)
            {
                Tree<Behaviour>.Node node = dragNode.Value;
                if (e.type == EventType.MouseUp && e.button == 0)
                {
                    if (hoverNode == null)
                    {
                        // Reset drag node
                        dragParent?.AddChild(node);
                        dragNode = null;
                    }
                    else
                    {
                        // Remove parent
                        dragParent?.RemoveChild(dragNode.Value);

                        // Add drag node to new parent
                        hoverNode?.AddChild(dragNode.Value);

                        dragNode = null;
                    }
                }
                else
                {
                    Rect r = new Rect(mousePos, new Vector2(100, 25));
                    GUI.Box(r, node.Element.Node.GetType().Name);
                }

                Repaint();
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                selectedTree.selectedNode = null;
                EditorUtility.SetDirty(selectedTree);
                Repaint();
            }
        }

        private void ShowNode(Tree<Behaviour>.Node? parent, Tree<Behaviour>.Node node, int indent = 0)
        {
            string name = node.Element.Node.GetType().Name;

            Rect layout = EditorGUILayout.BeginHorizontal();

            float spacing = (indent + 1) * INDENT_MULTIPLIER;
            GUILayout.Space(spacing);

            bool isCollapsed = collapsedNodes.Contains(node);
            if (GUILayout.Button(isCollapsed ? "+" : "-", GUILayout.Width(25)))
            {
                isCollapsed = !isCollapsed;
                if (isCollapsed)
                {
                    collapsedNodes.Add(node);
                }
                else
                {
                    collapsedNodes.Remove(node);
                }
            }

            layout.x += spacing + 29;
            layout.width -= spacing + 114;

            if (selectedTree.selectedNode == node.Element)
            {
                EditorUtils.DrawBox(layout, EditorUtils.HIGHLIGHTED_COLOR);
            }

            if (GUI.Button(layout, GUIContent.none, GUI.skin.box))
            {
                selectedTree.selectedNode = node.Element;
                EditorUtility.SetDirty(selectedTree);
            }

            if (layout.Contains(mousePos + scrollPos)) hoverNode = node;

            GUILayout.Label(name);

            if (parent != null && dragNode == null && GUILayout.RepeatButton("*", GUILayout.Width(25)))
            {
                dragNode = node;
                dragParent = parent;
                dragParent?.RemoveChild(dragNode.Value);
            }

            if (GUILayout.Button("+", GUILayout.Width(25))) AddChild(node);

            if (parent != null && GUILayout.Button("-", GUILayout.Width(25))) parent?.RemoveChild(node);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(LINE_GAP);

            if (isCollapsed) return;

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                ShowNode(node, child, indent + 1);
            }
        }

        private void AddChild(Tree<Behaviour>.Node parent)
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
                        Behaviour bNode = new Behaviour(node);
                        node.Serialize(bNode);
                        parent.AddChild(new Tree<Behaviour>.Node(bNode));
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
                    selectedTree.tree = new Tree<Behaviour>(new Behaviour(new SequenceNode()));
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

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            BehaviourTree behaviourTree = AssetDatabase.LoadAssetAtPath<BehaviourTree>(assetPath);
            if (behaviourTree != null)
            {
                CreateWindow();
                return true;
            }
            return false;
        }
    }
}
