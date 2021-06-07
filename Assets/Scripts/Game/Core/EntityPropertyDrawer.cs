using UnityEditor;
using UnityEngine;

namespace RPG
{
    [CustomPropertyDrawer(typeof(EntityProperty))]
    public class EntityPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var nameRect = new Rect(position.x, position.y, 90, position.height);
            var typeRect = new Rect(position.x + 92, position.y, 54, position.height);
            var valueRect = new Rect(position.x + 148, position.y, position.width - 148, position.height);

            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
            EditorGUI.PropertyField(valueRect, GetValueProperty(property), GUIContent.none);

            EditorGUI.EndProperty();
        }

        private SerializedProperty GetValueProperty(SerializedProperty property)
        {
            PropertyType type = (PropertyType)property.FindPropertyRelative("type").enumValueIndex;
            switch (type)
            {
                case PropertyType.Int: return property.FindPropertyRelative("iValue");
                case PropertyType.Float: return property.FindPropertyRelative("fValue");
                case PropertyType.String: return property.FindPropertyRelative("sValue");
                case PropertyType.Bool: return property.FindPropertyRelative("bValue");
                default: return property.FindPropertyRelative("sValue");
            }
        }
    }
}
