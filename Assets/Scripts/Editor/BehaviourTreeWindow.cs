using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Behaviours
{
    public class BehaviourTreeWindow : EditorWindow
    {
        private const float INDENT_MULTIPLIER = 20.0f;
        private const float LINE_GAP = 2.0f;

        private Vector2 CentreOfWindow { get => this.position.size / 2.0f; }
        private BehaviourTree selectedTree = null;
        private Vector2 treeScrollPos = Vector2.zero;
        private Vector2 listScrollPos = Vector2.zero;
        private Tree<Behaviour>.Node? dragNode = null;
        private Tree<Behaviour>.Node? dragParent = null;
        private Tree<Behaviour>.Node? hoverNode = null;
        private Tree<Behaviour>.Node? hoverParent = null;
        private BehaviourTreeNodeType? dragOption = null;
        private Vector2 mousePos;
        private BehaviourTreeNodeType[] nodeOptions;
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

            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            treeScrollPos = GUILayout.BeginScrollView(treeScrollPos);
            Tree<Behaviour>.Node root = selectedTree.Root;
            hoverNode = null;
            ShowNode(null, root);
            GUILayout.EndScrollView();

            listScrollPos = GUILayout.BeginScrollView(listScrollPos, GUILayout.Width(200f));
            ShowOptions();
            GUILayout.EndScrollView();

            GUILayout.EndHorizontal();

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
                    Vector2 boxSize = new Vector2(GetTextSize(node.Element.Node.GetType().Name).x + 20, 20);
                    Rect r = new Rect(mousePos, boxSize);
                    GUI.Box(r, node.Element.Node.GetType().Name);
                }

                Repaint();
            }

            if (dragOption != null)
            {
                if (e.type == EventType.MouseUp && e.button == 0)
                {
                    Tree<Behaviour>.Node parent = hoverNode.HasValue ? hoverNode.Value : selectedTree.Root;
                    AddChild(parent, dragOption.Value);
                    dragOption = null;
                }
                else
                {
                    Vector2 boxSize = new Vector2(GetTextSize(dragOption.ToString()).x + 20, 20);
                    Rect r = new Rect(mousePos, boxSize);
                    GUI.Box(r, dragOption.ToString());
                }

                Repaint();
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                selectedTree.selectedNode = null;
                EditorUtility.SetDirty(selectedTree);
                Repaint();
            }

            if (e.type == EventType.Used && e.button == 1)
            {
                ShowNodeMenu();
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
            layout.width -= spacing + 58;

            if (selectedTree.selectedNode == node.Element)
            {
                EditorUtils.DrawBox(layout, EditorUtils.HIGHLIGHTED_COLOR);
            }

            if (GUI.Button(layout, GUIContent.none, GUI.skin.box))
            {
                selectedTree.selectedNode = node.Element;
                EditorUtility.SetDirty(selectedTree);
            }

            if (layout.Contains(mousePos + treeScrollPos))
            {
                hoverNode = node;
                hoverParent = parent;
            }

            GUILayout.Label(name);

            if (parent != null && dragNode == null && GUILayout.RepeatButton("*", GUILayout.Width(25)))
            {
                dragNode = node;
                dragParent = parent;
                dragParent?.RemoveChild(dragNode.Value);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(LINE_GAP);

            if (isCollapsed) return;

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                ShowNode(node, child, indent + 1);
            }
        }

        private void ShowOptions()
        {
            if (dragOption.HasValue)
            {
                GUI.enabled = false;
            }

            foreach (BehaviourTreeNodeType type in nodeOptions)
            {
                if (GUILayout.RepeatButton(type.ToString()))
                {
                    dragOption = type;
                }
            }

            GUI.enabled = true;
        }

        private void AddChild(Tree<Behaviour>.Node parent, BehaviourTreeNodeType type)
        {
            IBehaviourTreeNode node = BehaviourTreeNodeCreator.Create(type);
            Behaviour bNode = new Behaviour(node);
            node.Serialize(bNode);
            parent.AddChild(new Tree<Behaviour>.Node(bNode));
            EditorUtility.SetDirty(selectedTree);
        }

        private void ShowNodeMenu()
        {
            if (!hoverNode.HasValue)
            {
                return;
            }

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => { hoverParent?.RemoveChild(hoverNode.Value); });
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

            nodeOptions = (BehaviourTreeNodeType[])System.Enum.GetValues(typeof(BehaviourTreeNodeType));

            Repaint();
        }

        private void ShowMessage(string message)
        {
            Vector2 position = CentreOfWindow;
            Vector2 size = GetTextSize(message);

            position.x -= size.x / 2.0f; //Centre the text
            GUI.Label(new Rect(position, size), message);
        }

        private Vector2 GetTextSize(string text)
        {
            GUIContent content = new GUIContent(text);
            return GUI.skin.label.CalcSize(content);
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
