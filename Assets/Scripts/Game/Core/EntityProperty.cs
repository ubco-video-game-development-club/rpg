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
    }
}
