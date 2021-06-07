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
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 21), EditorUtils.BORDER_COLOR);
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 20), EditorUtils.BACKGROUND_COLOR);
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

            ClassTier targetTier = selectedClassTree.GetTierAt(position);
            if (targetTier != null)
            {
                ClassNode targetNode = selectedClassTree.GetNodeAt(position);
                if (targetNode != null)
                {
                    menu.AddItem(
                        new GUIContent("Delete Node"),
                        false,
                        () =>
                        {
                            selectedClassTree.RemoveNode(targetTier.level, targetNode);
                            EditorUtility.SetDirty(selectedClassTree);
                        }
                    );
                }
                else
                {
                    // TODO: implement subclasses
                    menu.AddDisabledItem(
                        new GUIContent("Create Subclass Node")
                    );
                    menu.AddItem(
                        new GUIContent("Create Skill Node"),
                        false,
                        () =>
                        {
                            selectedClassTree.AddNode(targetTier.level, ClassNodeType.Skill);
                            EditorUtility.SetDirty(selectedClassTree);
                        }
                    );
                }

                menu.AddSeparator("");

                if (targetTier.level == 1)
                {
                    menu.AddDisabledItem(new GUIContent("Delete Tier"));
                }
                else
                {
                    menu.AddItem(
                        new GUIContent("Delete Tier"),
                        false,
                        () =>
                        {
                            selectedClassTree.RemoveTier(targetTier.level);
                            EditorUtility.SetDirty(selectedClassTree);
                        }
                    );
                }

                if (selectedClassTree.ContainsTier(targetTier.level + 1))
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
                            selectedClassTree.AddTier(targetTier.level + 1);
                            EditorUtility.SetDirty(selectedClassTree);
                        }
                    );
                }
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
