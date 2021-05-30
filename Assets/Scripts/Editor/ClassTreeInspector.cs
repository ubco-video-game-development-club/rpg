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
        private SerializedProperty tierProp;
        private SerializedProperty nodeProp;

        void OnEnable()
        {
            tree = serializedObject.targetObject as ClassTree;
            tierProp = serializedObject.FindProperty("selectedTier");
            nodeProp = serializedObject.FindProperty("selectedNode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (tree.selectedTier.isSelected)
            {
                DisplayTier();
            }
            else if (tree.selectedNode.isSelected)
            {
                // TODO: node
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void DisplayTier()
        {
            SerializedProperty levelProp = tierProp.FindPropertyRelative("level");
            EditorGUILayout.LabelField("Class Tier", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Level");
            int newLevel = EditorGUILayout.IntField(levelProp.intValue);
            if (!tree.ContainsTier(newLevel))
            {
                levelProp.intValue = newLevel;
                tree.MoveTier(tree.selectedTier.level, newLevel);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
