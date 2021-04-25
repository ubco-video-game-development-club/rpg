using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    [CustomEditor(typeof(BehaviourTree))]
    public class BehaviourTreeInspector : Editor
    {
        private BehaviourTree behaviourTree;
        private VariableProperty[] propertyBuffer;
        private BehaviourTreeNode lastSelected = null;

        void OnEnable()
        {
            behaviourTree = serializedObject.targetObject as BehaviourTree;
        }

        public override void OnInspectorGUI()
        {
            BehaviourTreeNode node = behaviourTree.selectedNode;
            if(node == null) return;
            if(propertyBuffer == null) propertyBuffer = behaviourTree.selectedNode.GetProperties();

            if(lastSelected != node)
            {
                propertyBuffer = behaviourTree.selectedNode.GetProperties();
                lastSelected = node;
            }

            string name = node.GetType().Name;
            GUILayout.Label(name);
            foreach(VariableProperty property in propertyBuffer)
            {
                DisplayProperty(property);
            }

            Repaint();
        }

        private void DisplayProperty(VariableProperty property)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(property.Name);

            switch (property.PropertyType)
            {
                case VariableProperty.Type.Boolean:
                    property.Set(GUILayout.Toggle(property.GetBoolean(), GUIContent.none));
                    break;
                case VariableProperty.Type.Number:
                    property.Set(EditorGUILayout.DoubleField(property.GetNumber()));
                    break;
                case VariableProperty.Type.Array:
                    Debug.Log("Not implemented!");
                    break;
                case VariableProperty.Type.String:
                    property.Set(GUILayout.TextField(property.GetString()));
                    break;
                }

            GUILayout.EndHorizontal();
        }
    }
}
