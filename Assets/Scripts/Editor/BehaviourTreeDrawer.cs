using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    [CustomPropertyDrawer(typeof(BehaviourTree))]
    public class BehaviourTreeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject owner = property.serializedObject;
            owner.Update();

            // TODO: add interface for any type that has a behaviour tree field; have Get/SetBehaviourTree() or something
            Agent agent = owner.targetObject as Agent;
            List<(string, VariableProperty)> instancedProps = GetInstancedProps(agent.BehaviourTree);

            EditorGUI.BeginProperty(position, label, property);

            Rect propRect = new Rect(position.x, position.y, position.width, 20);
            EditorGUI.PropertyField(propRect, property);

            Rect backgroundRect = new Rect(position.x + 50, position.y + 24, position.width - 100, instancedProps.Count * 20 + 23);
            EditorGUI.DrawRect(backgroundRect, EditorUtils.BORDER_COLOR);

            Rect fieldsRect = new Rect(position.x + 60, position.y + 20, position.width - 120, 20);
            EditorGUI.DropShadowLabel(fieldsRect, "Instance Properties");

            // TODO: ~~~~~ VERY IMPORTANT ~~~~~
            // This currently works perfectly to update the original tree itself...
            // However, we need it to be updated on the Agent, not the tree.
            // This should be a matter of storing a separate list of properties on the Agent (again, see the interface idea above)
            // The list should be generated from the tree as it currently is, but the values should be serialized on the agent
            fieldsRect.y += 4;
            bool changed = false;
            foreach ((string, VariableProperty) prop in instancedProps)
            {
                fieldsRect.y += 20;
                changed = DrawProperty(fieldsRect, prop.Item1, prop.Item2);
            }

            EditorGUI.EndProperty();

            owner.ApplyModifiedProperties();
            if (changed) EditorUtility.SetDirty(agent);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // TODO: add interface for any type that has a behaviour tree field; have Get/SetBehaviourTree() or something
            Agent agent = property.serializedObject.targetObject as Agent;
            List<(string, VariableProperty)> instancedProps = GetInstancedProps(agent.BehaviourTree);
            return base.GetPropertyHeight(property, label) + instancedProps.Count * 20 + 24;
        }

        private List<(string, VariableProperty)> GetInstancedProps(BehaviourTree behaviourTree)
        {
            List<(string, VariableProperty)> instancedProps = new List<(string, VariableProperty)>();
            Queue<Tree<Behaviour>.Node> nodes = new Queue<Tree<Behaviour>.Node>();
            nodes.Enqueue(behaviourTree.Root);
            while (nodes.Count > 0)
            {
                Tree<Behaviour>.Node node = nodes.Dequeue();

                foreach (string propName in node.Element.Properties.Keys)
                {
                    VariableProperty nodeProp = node.Element.GetProperty(propName);
                    if (nodeProp.Instanced)
                    {
                        instancedProps.Add((propName, nodeProp));
                    }
                }

                for (int i = 0; i < node.ChildCount; i++)
                {
                    nodes.Enqueue(node.GetChild(i));
                }
            }
            return instancedProps;
        }

        private bool DrawProperty(Rect position, string label, VariableProperty property)
        {
            // TODO: rather than property.Set() here, we want to save/read the value from the Agent's list.
            Rect contentRect = EditorGUI.PrefixLabel(position, new GUIContent(label));
            switch (property.PropertyType)
            {
                case VariableProperty.Type.Boolean:
                    bool bPrev = property.GetBoolean();
                    bool bNew = EditorGUI.Toggle(contentRect, bPrev);
                    property.Set(bNew);
                    return bPrev != bNew;
                case VariableProperty.Type.Number:
                    double nPrev = property.GetNumber();
                    double nNew = EditorGUI.DoubleField(contentRect, nPrev);
                    property.Set(nNew);
                    return nPrev != nNew;
                case VariableProperty.Type.String:
                    string sPrev = property.GetString();
                    string sNew = EditorGUI.TextField(contentRect, sPrev);
                    property.Set(sNew);
                    return sPrev != sNew;
                case VariableProperty.Type.Object:
                    Object oPrev = property.GetObject();
                    Object oNew = EditorGUI.ObjectField(contentRect, oPrev, property.ObjectType, true);
                    property.Set(oNew);
                    return oPrev != oNew;
                case VariableProperty.Type.Vector:
                    Vector2 vPrev = property.GetVector();
                    Vector2 vNew = EditorGUI.Vector2Field(contentRect, "", vPrev);
                    property.Set(vNew);
                    return vPrev != vNew;
                case VariableProperty.Type.Enum:
                    int ePrev = property.GetEnum();
                    System.Enum eVal = (System.Enum)System.Enum.ToObject(property.ObjectType, ePrev);
                    string sResult = EditorGUI.EnumPopup(contentRect, eVal).ToString();
                    int eNew = (int)System.Enum.Parse(property.ObjectType, sResult);
                    property.Set(eNew);
                    return ePrev != eNew;
            }
            return false;
        }
    }
}
