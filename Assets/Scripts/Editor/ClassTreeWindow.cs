using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    public class ClassTreeWindow : EditorWindow
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
            ClassTreeWindow window = EditorWindow.GetWindow<ClassTreeWindow>();
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
                float y = 31f;
                float h = (selectedClassTree.LevelUpTiers.Count + 1) * (NODE_HEIGHT + 4f + TIER_SPACING);
                scrollPosition = GUI.BeginScrollView(new Rect(0, 21f, Screen.width - 2f, Screen.height - 42f), scrollPosition, new Rect(0, 21f, Screen.width - 15f, h));
                foreach (KeyValuePair<int, List<ClassTreeNode>> tier in selectedClassTree.LevelUpTiers)
                {
                    int level = tier.Key;
                    List<ClassTreeNode> nodes = tier.Value;

                    foreach (ClassTreeNode node in nodes)
                    {
                        // TODO: Spacing here!
                        EditorUtils.DrawBorderBox(new Rect((Screen.width - MARGIN_WIDTH) / 2 - NODE_WIDTH / 2, y + 2f, NODE_WIDTH, NODE_HEIGHT), HEADER_COLOR, 1, DIVIDER_COLOR);
                    }

                    GUI.Label(new Rect(Screen.width - MARGIN_WIDTH + 10f, y + 2f, MARGIN_WIDTH - 20f, NODE_HEIGHT), $"Level {level}");

                    y += NODE_HEIGHT + 4f + TIER_SPACING;
                }
                GUI.Button(new Rect(Screen.width - MARGIN_WIDTH / 2 - 40f, y + 2f, 80f, 25f), "Add Tier");
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
                selectedClassTree.AddNode(1, new ClassTreeNode());
                selectedClassTree.AddTier(2);
                selectedClassTree.AddNode(2, new ClassTreeNode());
                selectedClassTree.AddTier(3);
                selectedClassTree.AddNode(3, new ClassTreeNode());
                selectedClassTree.AddTier(4);
                selectedClassTree.AddNode(4, new ClassTreeNode());
                selectedClassTree.AddTier(5);
                selectedClassTree.AddNode(5, new ClassTreeNode());
                selectedClassTree.AddTier(6);
                selectedClassTree.AddNode(6, new ClassTreeNode());
                selectedClassTree.AddTier(7);
                selectedClassTree.AddNode(7, new ClassTreeNode());
                selectedClassTree.AddTier(8);
                selectedClassTree.AddNode(8, new ClassTreeNode());
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
