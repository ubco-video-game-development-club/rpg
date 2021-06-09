using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    public class ClassEditorWindow : EditorWindow
    {
        private ClassTree selectedTree;
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
            // Draw header
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 21), EditorUtils.BORDER_COLOR);
            EditorUtils.DrawBox(new Rect(0, 0, Screen.width, 20), EditorUtils.BACKGROUND_COLOR);
            GUILayout.Label(selectedAssetPath != "" ? selectedAssetPath : "No Class Tree selected!");

            if (selectedTree != null)
            {
                ProcessEvents(Event.current);
                selectedTree.Draw(new Rect(0, 21, Screen.width, Screen.height - 21));
            }
        }

        private void ProcessEvents(Event e)
        {
            if (selectedTree.IsEditingPath) return;

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

            ClassTier targetTier = selectedTree.GetTierAt(position);
            if (targetTier != null)
            {
                ClassNode targetNode = selectedTree.GetNodeAt(position);
                if (targetNode != null)
                {
                    if (targetTier.level == 1)
                    {
                        menu.AddDisabledItem(new GUIContent("Delete Node"));
                    }
                    else
                    {
                        menu.AddItem(
                            new GUIContent("Delete Node"),
                            false,
                            () =>
                            {
                                selectedTree.RemoveNode(targetNode);
                                EditorUtility.SetDirty(selectedTree);
                            }
                        );
                    }
                }
                else
                {
                    if (targetTier.level == 1)
                    {
                        menu.AddDisabledItem(new GUIContent("Create Subclass Node"));
                        menu.AddDisabledItem(new GUIContent("Create Skill Node"));
                    }
                    else
                    {
                        // TODO: implement subclasses
                        menu.AddDisabledItem(new GUIContent("Create Subclass Node"));
                        menu.AddItem(
                            new GUIContent("Create Skill Node"),
                            false,
                            () =>
                            {
                                selectedTree.AddNode(targetTier.level, ClassNodeType.Skill);
                                EditorUtility.SetDirty(selectedTree);
                            }
                        );
                    }
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
                            selectedTree.RemoveTier(targetTier.level);
                            EditorUtility.SetDirty(selectedTree);
                        }
                    );
                }

                if (selectedTree.ContainsTier(targetTier.level + 1))
                {
                    menu.AddDisabledItem(new GUIContent("Insert Tier Below"));
                }
                else
                {
                    menu.AddItem(
                        new GUIContent("Insert Tier Below"),
                        false,
                        () =>
                        {
                            selectedTree.AddTier(targetTier.level + 1);
                            EditorUtility.SetDirty(selectedTree);
                        }
                    );
                }
            }
            else
            {
                menu.AddItem(
                    new GUIContent("New Tier"),
                    false,
                    () =>
                    {
                        int highestLevel = 0;
                        foreach (ClassTier tier in selectedTree.Layers.Values)
                        {
                            if (tier.level > highestLevel) highestLevel = tier.level;
                        }
                        selectedTree.AddTier(highestLevel + 1);
                        EditorUtility.SetDirty(selectedTree);
                    }
                );
            }

            menu.ShowAsContext();
        }

        private void UpdateSelectedTree()
        {
            if (Selection.activeObject is ClassTree)
            {
                selectedTree = Selection.activeObject as ClassTree;
                selectedAssetPath = AssetDatabase.GetAssetPath(selectedTree.GetInstanceID());

                if (!selectedTree.ContainsTier(1))
                {
                    selectedTree.AddTier(1);
                    selectedTree.AddNode(1, ClassNodeType.Class);
                }

                selectedTree.Initialize();
            }
            else
            {
                selectedTree = null;
                selectedAssetPath = "";
            }

            Repaint();
        }
    }
}
