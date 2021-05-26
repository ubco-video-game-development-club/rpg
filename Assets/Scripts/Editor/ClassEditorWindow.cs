using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    public class ClassEditorWindow : EditorWindow
    {
        private const float TIER_SPACING = 50f;
        private const float NODE_WIDTH = 80f;
        private const float NODE_HEIGHT = 50f;
        private const float NODE_SPACING = 5f;
        private const float MARGIN_WIDTH = 140f;

        private ClassTree selectedClassTree;
        private string selectedAssetPath;
        private Vector2 scrollPosition;

        [MenuItem("Window/Class Editor")]
        public static void ShowWindow()
        {
            ClassEditorWindow window = EditorWindow.GetWindow<ClassEditorWindow>();
            window.titleContent = new GUIContent("Class Editor");
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            ClassTree classTree = AssetDatabase.LoadAssetAtPath<ClassTree>(assetPath);
            if (classTree != null)
            {
                ShowWindow();
                return true;
            }
            return false;
        }

        void OnEnable()
        {
            UpdateSelectedTree();
        }

        void OnSelectionChange()
        {
            UpdateSelectedTree();
        }

        void OnGUI()
        {
            Color HEADER_COLOR = new Color(0.235f, 0.235f, 0.235f);
            Color DIVIDER_COLOR = new Color(0.137f, 0.137f, 0.137f);

            // Draw header
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 21f), DIVIDER_COLOR);
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 20f), HEADER_COLOR);
            GUILayout.Label(selectedAssetPath != "" ? selectedAssetPath : "No Class Tree selected!");

            if (selectedClassTree != null)
            {
                // Draw levelup tiers
                float y = 21f + TIER_SPACING / 2f;
                float h = (selectedClassTree.LevelUpTiers.Count + 1) * (NODE_HEIGHT + 4f + TIER_SPACING);
                scrollPosition = GUI.BeginScrollView(new Rect(0, 21f, Screen.width - 2f, Screen.height - 42f), scrollPosition, new Rect(0, 21f, Screen.width - 15f, h));
                foreach (KeyValuePair<int, List<ClassTreeNode>> tier in selectedClassTree.LevelUpTiers)
                {
                    int level = tier.Key;
                    List<ClassTreeNode> nodes = tier.Value;

                    float centerX = (Screen.width - MARGIN_WIDTH) / 2;
                    foreach (ClassTreeNode node in nodes)
                    {
                        // TODO: Spacing here!
                        EditorUtils.DrawBorderBox(new Rect(centerX - NODE_WIDTH / 2, y, NODE_WIDTH, NODE_HEIGHT), HEADER_COLOR, 1, DIVIDER_COLOR);

                        if (GUI.Button(new Rect(centerX + NODE_WIDTH / 2 - 10f, y - NODE_HEIGHT / 2 + 15f, 18f, 18f), "X"))
                        {
                            selectedClassTree.AddTier(level + 1);
                        }
                    }

                    if (GUI.Button(new Rect(centerX + NODE_WIDTH / 2 + 50f, y + NODE_HEIGHT / 2f - 10f, 80f, 20f), "Add Node"))
                    {
                        selectedClassTree.AddTier(level + 1);
                    }

                    EditorUtils.DrawBox(new Rect(0, y + NODE_HEIGHT + TIER_SPACING / 2, Screen.width, 2f), HEADER_COLOR);

                    if (!selectedClassTree.ContainsTier(level + 1))
                    {
                        if (GUI.Button(new Rect(centerX - 10f, y + NODE_HEIGHT + TIER_SPACING / 2 - 10f, 20f, 20f), "+"))
                        {
                            selectedClassTree.AddTier(level + 1);
                        }
                    }

                    GUI.Label(new Rect(Screen.width - MARGIN_WIDTH + 20f, y + 2f, 80f, NODE_HEIGHT), $"Level {level}");

                    if (GUI.Button(new Rect(Screen.width - MARGIN_WIDTH + 109f, y - NODE_HEIGHT / 2 + 15f, 18f, 18f), "X"))
                    {
                        selectedClassTree.AddTier(level + 1);
                    }

                    y += NODE_HEIGHT + 4f + TIER_SPACING;
                }

                GUI.EndScrollView();

                // Draw vertical divider
                EditorUtils.DrawBox(new Rect(Screen.width - MARGIN_WIDTH + 1f, 21f, 2f, Screen.height - 21f), DIVIDER_COLOR);
            }
        }

        private void UpdateSelectedTree()
        {
            if (Selection.activeObject is ClassTree)
            {
                selectedClassTree = Selection.activeObject as ClassTree;
                selectedClassTree.AddTier(1);
                selectedClassTree.AddNode(1, new ClassTreeNode(Vector2.zero));
                selectedClassTree.AddTier(3);
                selectedClassTree.AddNode(3, new ClassTreeNode(Vector2.zero));
                selectedClassTree.AddTier(5);
                selectedClassTree.AddNode(5, new ClassTreeNode(Vector2.zero));
                selectedClassTree.AddTier(8);
                selectedClassTree.AddNode(8, new ClassTreeNode(Vector2.zero));
                selectedAssetPath = AssetDatabase.GetAssetPath(selectedClassTree.GetInstanceID());
            }
            else
            {
                selectedClassTree = null;
                selectedAssetPath = "";
            }

            Repaint();
        }
    }
}
