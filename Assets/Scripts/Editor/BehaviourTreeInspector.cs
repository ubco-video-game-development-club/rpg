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
        private Behaviour selected = null;

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
            if (selected == null || selected != node)
            {
                isArrayExpanded = false;
                arrayLength = 0;
                arr = null;
            }
            selected = node;

            ClearArrayData();

            string name = node.Node.GetType().Name;
            GUILayout.Label(name);
            foreach (string propertyName in node.Properties.Keys)
            {
                bool reserializing = DisplayProperty(propertyName, node.Properties[propertyName]);
                if (reserializing) break;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(behaviourTree);
            Repaint();
        }

        private bool DisplayProperty(string name, VariableProperty property)
        {
            if (property.PropertyType == VariableProperty.Type.Array)
            {
                DisplayArray(name, property);
                return false;
            }

            GUILayout.BeginHorizontal();

            property.Instanced = GUILayout.Toggle(property.Instanced, "", GUILayout.Width(20));

            GUI.enabled = !property.Instanced;
            GUILayout.Label(name);

            if (property.Instanced)
            {
                GUILayout.Label("[Instanced]");
            }
            else
            {
                switch (property.PropertyType)
                {
                    case VariableProperty.Type.Boolean:
                        bool bPrev = property.GetBoolean();
                        bool bNew = GUILayout.Toggle(bPrev, GUIContent.none);
                        property.Set(bNew);
                        if (property.ForceReserialization && bPrev != bNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                    case VariableProperty.Type.Number:
                        double nPrev = property.GetNumber();
                        double nNew = EditorGUILayout.DoubleField(nPrev);
                        property.Set(nNew);
                        if (property.ForceReserialization && nPrev != nNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                    case VariableProperty.Type.String:
                        string sPrev = property.GetString();
                        string sNew = GUILayout.TextField(sPrev);
                        property.Set(sNew);
                        if (property.ForceReserialization && sPrev != sNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                    case VariableProperty.Type.Object:
                        Object oPrev = property.GetObject();
                        Object oNew = EditorGUILayout.ObjectField(oPrev, property.ObjectType, true);
                        property.Set(oNew);
                        if (property.ForceReserialization && oPrev != oNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                    case VariableProperty.Type.Vector:
                        Vector2 vPrev = property.GetVector();
                        Vector2 vNew = EditorGUILayout.Vector2Field("", vPrev);
                        property.Set(vNew);
                        if (property.ForceReserialization && vPrev != vNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                    case VariableProperty.Type.Enum:
                        int ePrev = property.GetEnum();
                        System.Enum eVal = (System.Enum)System.Enum.ToObject(property.ObjectType, ePrev);
                        string sResult = EditorGUILayout.EnumPopup(eVal).ToString();
                        int eNew = (int)System.Enum.Parse(property.ObjectType, sResult);
                        property.Set(eNew);
                        if (property.ForceReserialization && ePrev != eNew)
                        {
                            selected.Node.Serialize(selected);
                            return true;
                        }
                        break;
                }
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();

            return false;
        }

        private void DisplayArray(string name, VariableProperty property)
        {
            int initialLength = arrayLength;
            if (arr == null)
            {
                object[] propArr = property.GetArray();
                arr = propArr;
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
                            arr[i] = EditorGUILayout.ObjectField((Object)arr[i], property.ObjectType, true);
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
