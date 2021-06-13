using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    [CustomEditor(typeof(ClassTree))]
    public class ClassTreeInspector : Editor
    {
        private ClassTree tree;
        private SerializedProperty layers;

        void OnEnable()
        {
            tree = serializedObject.targetObject as ClassTree;
            layers = serializedObject.FindProperty("layers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (tree.selectedNodeIndex >= 0)
            {
                DisplayNode();
            }
            else if (tree.selectedLevel >= 0)
            {
                DisplayTier();
            }

            if (!EditorUtility.IsDirty(tree))
            {
                serializedObject.ApplyModifiedProperties();
            }
            Repaint();
        }

        private void DisplayTier()
        {
            int levelIdx = tree.IndexOfLevel(tree.selectedLevel);
            SerializedProperty levelsProp = layers.FindPropertyRelative("levels");
            SerializedProperty levelKeyProp = levelsProp.GetArrayElementAtIndex(levelIdx);

            SerializedProperty tierProp = GetSelectedTierProperty();
            SerializedProperty levelProp = tierProp.FindPropertyRelative("level");

            EditorGUILayout.LabelField("Class Tier", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Level");
            int currentLevel = tree.selectedLevel;
            GUI.enabled = currentLevel == 1 ? false : true;
            int newLevel = EditorGUILayout.IntField(levelProp.intValue);
            GUI.enabled = true;
            if (newLevel > 0 && !tree.ContainsTier(newLevel))
            {
                tree.MoveTier(currentLevel, newLevel);
                tree.selectedLevel = newLevel;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayNode()
        {
            SerializedProperty tierProp = GetSelectedTierProperty();
            SerializedProperty nodesProp = tierProp.FindPropertyRelative("nodes");
            SerializedProperty nodeProp = nodesProp.GetArrayElementAtIndex(tree.selectedNodeIndex);

            switch (tree.selectedNode.nodeType)
            {
                case ClassNodeType.Class:
                    SerializedProperty classDataProp = nodeProp.FindPropertyRelative("classData");
                    SerializedProperty healthProp = classDataProp.FindPropertyRelative("health");
                    EditorGUILayout.LabelField("Class Node", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(healthProp);
                    break;
                case ClassNodeType.Skill:
                    SerializedProperty levelUpOptionsProp = nodeProp.FindPropertyRelative("levelUpOptions");
                    EditorGUILayout.LabelField("Skill Node", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(levelUpOptionsProp, GUIContent.none);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty GetSelectedTierProperty()
        {
            int levelIdx = tree.IndexOfLevel(tree.selectedLevel);
            SerializedProperty tiersProp = layers.FindPropertyRelative("tiers");
            return tiersProp.GetArrayElementAtIndex(levelIdx);
        }
    }
}
