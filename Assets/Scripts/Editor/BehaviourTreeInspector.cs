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

        void OnEnable()
        {
            behaviourTree = serializedObject.targetObject as BehaviourTree;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Behaviour node = behaviourTree.selectedNode;
            Debug.Log(node);
            if (node == null) return;

            string name = node.Node.GetType().Name;
            GUILayout.Label(name);
            foreach (string propertyName in node.Properties.Keys)
            {
                DisplayProperty(propertyName, node.Properties[propertyName]);
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(behaviourTree);
            Repaint();
        }

        private void DisplayProperty(string name, VariableProperty property)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);

            switch (property.PropertyType)
            {
                case VariableProperty.Type.Boolean:
                    property.Set(GUILayout.Toggle(property.GetBoolean(), GUIContent.none));
                    break;
                case VariableProperty.Type.Number:
                    property.Set(EditorGUILayout.DoubleField(property.GetNumber()));
                    break;
                case VariableProperty.Type.String:
                    property.Set(GUILayout.TextField(property.GetString()));
                    break;
                case VariableProperty.Type.Object:
                    property.Set(EditorGUILayout.ObjectField(property.GetObject(), property.GetObjectType(), true));
                    break;
                case VariableProperty.Type.Vector:
                    property.Set(EditorGUILayout.Vector2Field("", property.GetVector()));
                    break;
            }

            GUILayout.EndHorizontal();
        }
    }
}
