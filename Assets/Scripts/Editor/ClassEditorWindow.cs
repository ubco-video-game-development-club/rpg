using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    public class ClassEditorWindow : EditorWindow
    {
        private ClassTree selectedClassTree;
        private string selectedAssetPath;

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
            ProcessEvents(Event.current);

            // Draw header
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 21), EditorUtils.DIVIDER_COLOR);
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 20), EditorUtils.HEADER_COLOR);
            GUILayout.Label(selectedAssetPath != "" ? selectedAssetPath : "No Class Tree selected!");

            if (selectedClassTree != null)
            {
                selectedClassTree.Draw(new Rect(0, 21, Screen.width, Screen.height - 21));
            }
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 1)
                {
                    CreateContextMenu(e.mousePosition);
                    e.Use();
                }
            }
        }

        private void CreateContextMenu(Vector2 position)
        {
            GenericMenu menu = new GenericMenu();

            ClassTreeTier selectedTier = selectedClassTree.GetTierAt(position);
            if (selectedTier != null)
            {
                if (selectedClassTree.ContainsTier(selectedTier.Level + 1))
                {
                    menu.AddDisabledItem(new GUIContent("Add Tier Below"));
                }
                else
                {
                    menu.AddItem(
                        new GUIContent("Add Tier Below"),
                        false,
                        () =>
                        {
                            selectedClassTree.AddTier(selectedTier.Level + 1);
                            EditorUtility.SetDirty(selectedClassTree);
                        }
                    );
                }

                menu.AddItem(
                    new GUIContent("Create Node"),
                    false,
                    () =>
                    {
                        selectedClassTree.AddNode(selectedTier.Level, new ClassTreeNode());
                        EditorUtility.SetDirty(selectedClassTree);
                    }
                );
            }

            menu.ShowAsContext();
        }

        private void UpdateSelectedTree()
        {
            if (Selection.activeObject is ClassTree)
            {
                selectedClassTree = Selection.activeObject as ClassTree;
                selectedAssetPath = AssetDatabase.GetAssetPath(selectedClassTree.GetInstanceID());

                if (!selectedClassTree.ContainsTier(1))
                {
                    selectedClassTree.AddTier(1);
                }
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
