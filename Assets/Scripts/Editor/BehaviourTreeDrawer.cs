using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Behaviours
{
    [CustomPropertyDrawer(typeof(BehaviourTree))]
    public class BehaviourTreeDrawer : PropertyDrawer
    {
        /// ~~~  TODO  ~~~
        /// behaviour nodes need to be able to read the instance properties from their behaviour instance
        /// do we pass these in as a second reference to the Tick() method? this is pretty decent
        /// another issue... the serializedObject may not reference the IBehaviourInstance directly (see dialogue...)
        /// does this remove the usefulness of IBehaviourInstance entirely? are we better off making it entirely case-based?
        /// or do we still need IBehaviourInstance for handling behaviour node logic anyway?

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject owner = property.serializedObject;
            owner.Update();

            EditorGUI.BeginProperty(position, label, property);

            Rect propRect = new Rect(position.x, position.y, position.width, 20);
            EditorGUI.PropertyField(propRect, property);

            bool changed = false;
            IBehaviourInstance behaviourInstance = owner.targetObject as IBehaviourInstance;
            BehaviourTree behaviourTree = behaviourInstance.GetBehaviourTree();
            if (behaviourTree != null)
            {
                changed = behaviourTree.RefreshInstance(behaviourInstance);
                BehaviourInstanceProperty[] instanceProperties = behaviourInstance.GetInstanceProperties();

                Rect backgroundRect = new Rect(position.x + 50, position.y + 24, position.width - 100, instanceProperties.Length * 20 + 23);
                EditorGUI.DrawRect(backgroundRect, EditorUtils.BORDER_COLOR);

                Rect fieldsRect = new Rect(position.x + 60, position.y + 20, position.width - 120, 20);
                EditorGUI.DropShadowLabel(fieldsRect, "Instance Properties");

                fieldsRect.y += 4;
                foreach (BehaviourInstanceProperty instanceProperty in instanceProperties)
                {
                    fieldsRect.y += 20;
                    if (DrawInstanceProperty(fieldsRect, instanceProperty))
                    {
                        changed = true;
                    }
                }
            }
            else
            {
                changed = behaviourInstance.GetInstanceProperties().Length > 0;
                behaviourInstance.SetInstanceProperties(new BehaviourInstanceProperty[0]);
            }

            EditorGUI.EndProperty();

            owner.ApplyModifiedProperties();
            if (changed) EditorUtility.SetDirty(owner.targetObject);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            IBehaviourInstance behaviourInstance = property.serializedObject.targetObject as IBehaviourInstance;
            int instancePropsHeight = behaviourInstance.GetBehaviourTree() != null ? behaviourInstance.GetInstanceProperties().Length * 20 + 24 : 0;
            return base.GetPropertyHeight(property, label) + instancePropsHeight;
        }

        private bool DrawInstanceProperty(Rect position, BehaviourInstanceProperty instanceProperty)
        {
            Rect contentRect = EditorGUI.PrefixLabel(position, new GUIContent(instanceProperty.name));
            VariableProperty property = instanceProperty.value;
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
