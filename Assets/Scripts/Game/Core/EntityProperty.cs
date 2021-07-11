using System;
using UnityEngine;

namespace RPG
{
    public enum PropertyType
    {
        Int, Float, String, Bool
    }

    [Serializable]
    public class EntityProperty
    {
        public PropertyName Name { get => name; }
        public PropertyType Type { get => type; }
        public dynamic Value
        {
            get
            {
                switch (type)
                {
                    case PropertyType.Int: return iValue;
                    case PropertyType.Float: return fValue;
                    case PropertyType.String: return sValue;
                    case PropertyType.Bool: return bValue;
                    default: return 0;
                }
            }
        }

        [SerializeField] private PropertyName name;
        [SerializeField] private PropertyType type;
        [SerializeField] private int iValue;
        [SerializeField] private float fValue;
        [SerializeField] private string sValue;
        [SerializeField] private bool bValue;

        public EntityProperty(PropertyName name, PropertyType type, dynamic value)
        {
            this.name = name;
            this.type = type;
            switch (type)
            {
                case PropertyType.Int:
                    iValue = value;
                    break;
                case PropertyType.Float:
                    fValue = value;
                    break;
                case PropertyType.String:
                    sValue = value;
                    break;
                case PropertyType.Bool:
                    bValue = value;
                    break;
                default:
                    Debug.LogError("Property must be a valid type!");
                    break;
            }
        }

        public void ApplyTo(Entity entity)
        {
            switch (Type)
            {
                case PropertyType.Int:
                    int currentIntValue = entity.GetProperty<int>(Name);
                    entity.SetProperty<int>(Name, currentIntValue + Value);
                    break;
                case PropertyType.Float:
                    float currentFloatValue = entity.GetProperty<float>(Name);
                    entity.SetProperty<float>(Name, currentFloatValue + Value);
                    break;
                default:
                    Debug.LogError("Property must be a valid type!");
                    break;
            }
        }
    }
}
