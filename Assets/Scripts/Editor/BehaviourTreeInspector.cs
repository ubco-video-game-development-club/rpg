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

        private bool isArrayExpanded = false;
        private int arrayLength = 0;
        private object[] arr;

        void OnEnable()
        {
            behaviourTree = serializedObject.targetObject as BehaviourTree;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Behaviour node = behaviourTree.selectedNode;
            if (node == null) return;

            ClearArrayData();

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
            if (property.PropertyType == VariableProperty.Type.Array)
            {
                DisplayArray(name, property);
                return;
            }

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

        private void DisplayArray(string name, VariableProperty property)
        {
            int initialLength = arrayLength;
            if (arr == null)
            {
                object[] propArr = property.GetArray();
                arr = new object[propArr.Length];
                for (int i = 0; i < propArr.Length; i++)
                {
                    arr[i] = propArr[i];
                }
                arrayLength = propArr.Length;
            }

            GUILayout.BeginHorizontal();
            isArrayExpanded = EditorGUILayout.Foldout(isArrayExpanded, name);
            arrayLength = EditorGUILayout.IntField(arrayLength);
            GUILayout.EndHorizontal();

            if (arrayLength != initialLength)
            {
                object[] tmp = new object[arrayLength];
                for (int i = 0; i < arrayLength; i++)
                {
                    if (i < arr.Length)
                    {
                        tmp[i] = arr[i];
                    }
                    else
                    {
                        switch (property.GetArrayType())
                        {
                            case VariableProperty.Type.Boolean:
                                tmp[i] = false;
                                break;
                            case VariableProperty.Type.Number:
                                tmp[i] = 0.0;
                                break;
                            case VariableProperty.Type.String:
                                tmp[i] = "";
                                break;
                            case VariableProperty.Type.Object:
                                tmp[i] = null;
                                break;
                            case VariableProperty.Type.Vector:
                                tmp[i] = Vector2.zero;
                                break;
                        }
                    }
                }
                arr = tmp;
            }

            if (isArrayExpanded)
            {
                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.BeginVertical();

                for (int i = 0; i < arrayLength; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Element " + i);
                    switch (property.GetArrayType())
                    {
                        case VariableProperty.Type.Boolean:
                            arr[i] = GUILayout.Toggle((bool)arr[i], GUIContent.none);
                            break;
                        case VariableProperty.Type.Number:
                            arr[i] = EditorGUILayout.DoubleField((double)arr[i]);
                            break;
                        case VariableProperty.Type.String:
                            arr[i] = GUILayout.TextField((string)arr[i]);
                            break;
                        case VariableProperty.Type.Object:
                            arr[i] = EditorGUILayout.ObjectField((Object)arr[i], property.GetObjectType(), true);
                            break;
                        case VariableProperty.Type.Vector:
                            arr[i] = EditorGUILayout.Vector2Field("", (Vector2)arr[i]);
                            break;
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            property.Set(arr);
        }

        private void ClearArrayData()
        {
            bool hasArrayProp = false;
            Behaviour node = behaviourTree.selectedNode;
            foreach (VariableProperty prop in node.Properties.Values)
            {
                if (prop.PropertyType == VariableProperty.Type.Array)
                {
                    hasArrayProp = true;
                    break;
                }
            }

            if (!hasArrayProp)
            {
                isArrayExpanded = false;
                arrayLength = 0;
                arr = null;
            }
        }
    }
}
