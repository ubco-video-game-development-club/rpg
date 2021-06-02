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

            if (tree.selectedLevel >= 0)
            {
                DisplayTier();
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void DisplayTier()
        {
            int levelIdx = tree.IndexOfLevel(tree.selectedLevel);
            SerializedProperty levelsProp = layers.FindPropertyRelative("levels");
            SerializedProperty tiersProp = layers.FindPropertyRelative("tiers");
            SerializedProperty levelKeyProp = levelsProp.GetArrayElementAtIndex(levelIdx);
            SerializedProperty tierProp = tiersProp.GetArrayElementAtIndex(levelIdx);
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
                levelKeyProp.intValue = newLevel;
                levelProp.intValue = newLevel;
                tree.MoveTier(currentLevel, newLevel);
                tree.selectedLevel = newLevel;
                Debug.Log("Here is the boi we just moved");
                tree.Print();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
