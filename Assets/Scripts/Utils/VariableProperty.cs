using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class VariableProperty
{
    [SerializeField] private Type propertyType;
    public Type PropertyType
    {
        get => propertyType;
        private set => propertyType = value;
    }

    [SerializeField] private string objectType;
    public System.Type ObjectType
    {
        get => System.Type.GetType(objectType);
        private set => objectType = value.AssemblyQualifiedName;
    }

    [SerializeField] private bool forceReserialization;
    public bool ForceReserialization
    {
        get => forceReserialization;
        set => forceReserialization = value;
    }

    [SerializeField] private Display displayType;
    public Display DisplayType
    {
        get => displayType;
        set => displayType = value;
    }

    [SerializeField] private bool instanced;
    public bool Instanced
    {
        get => instanced;
        set => instanced = value;
    }

    [SerializeField] private Value value;
    [SerializeField] private Type aType;

    public VariableProperty(Type type)
    {
        if (type == Type.Object) Debug.LogError("ERROR: Attempting to use Object-based VariableProperty constructor with no Object type! You must specify the Object type!");
        if (type == Type.Array) Debug.LogError("ERROR: Attempting to use Array-based VariableProperty constructor with no Array type! You must specify the Array type!");
        PropertyType = type;
        value = new Value();
    }

    public VariableProperty(Type type, System.Type objectType)
    {
        if (type != Type.Object && type != Type.Enum) Debug.LogError("ERROR: Attempting to use Object/Enum-based VariableProperty constructor with non-Object/Enum type!");
        PropertyType = type;
        ObjectType = objectType;
        value = new Value();
        if (type == Type.Enum) Set(0);
    }

    public VariableProperty(Type type, Type arrayType)
    {
        if (type != Type.Array) Debug.LogError("ERROR: Attempting to use Array-based VariableProperty constructor with non-Array type!");
        if (arrayType == Type.Object) Debug.LogError("ERROR: Attempting to use Array-of-Objects-based VariableProperty constructor with no Object type! You must specify the Object type!");
        if (arrayType == Type.Array) Debug.LogError("ERROR: VariableProperty Array type cannot have nested Arrays!");
        PropertyType = Type.Array;
        aType = arrayType;
        value = new Value();
    }

    public VariableProperty(Type type, Type arrayType, System.Type objectType)
    {
        if (type != Type.Array) Debug.LogError("ERROR: Attempting to use Array-based VariableProperty constructor with non-Array type!");
        if (arrayType == Type.Array) Debug.LogError("ERROR: VariableProperty Array type cannot have nested Arrays!");
        PropertyType = type;
        aType = arrayType;
        ObjectType = objectType;
        value = new Value();
    }

    public bool GetBoolean()
    {
        if (PropertyType != Type.Boolean) throw new InvalidOperationException("This property is not a boolean type.");
        return value.b;
    }

    public void Set(bool value)
    {
        if (PropertyType != Type.Boolean) throw new InvalidOperationException("This property is not a boolean type.");
        this.value.b = value;
    }

    public double GetNumber()
    {
        if (PropertyType != Type.Number) throw new InvalidOperationException("This property is not a number type.");
        return value.n;
    }

    public void Set(double value)
    {
        if (PropertyType != Type.Number) throw new InvalidOperationException("This property is not a number type.");
        this.value.n = value;
    }

    public string GetString()
    {
        if (PropertyType != Type.String) throw new InvalidOperationException("This property is not a string type.");
        return value.s;
    }

    public void Set(string value)
    {
        if (PropertyType != Type.String) throw new InvalidOperationException("This property is not a string type.");
        this.value.s = value;
    }

    public UnityEngine.Object GetObject()
    {
        if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
        return value.o;
    }

    public void Set(UnityEngine.Object value)
    {
        if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
        this.value.o = value;
    }

    public Vector2 GetVector()
    {
        if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not a vector type.");
        return value.v;
    }

    public void Set(Vector2 value)
    {
        if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not a vector type.");
        this.value.v = value;
    }

    public object[] GetArray()
    {
        if (PropertyType != Type.Array) throw new InvalidOperationException("This property is not an array type.");

        VariablePropertyBase[] arr = value.a;
        if (arr == null) return new object[0];

        object[] result = new object[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            switch (arr[i].PropertyType)
            {
                case VariableProperty.Type.Boolean:
                    result[i] = arr[i].GetBoolean();
                    break;
                case VariableProperty.Type.Number:
                    result[i] = arr[i].GetNumber();
                    break;
                case VariableProperty.Type.String:
                    result[i] = arr[i].GetString();
                    break;
                case VariableProperty.Type.Object:
                    result[i] = arr[i].GetObject();
                    break;
                case VariableProperty.Type.Vector:
                    result[i] = arr[i].GetVector();
                    break;
            }
        }
        return result;
    }

    public Type GetArrayType()
    {
        if (PropertyType != Type.Array) throw new InvalidOperationException("This property is not an array type.");
        return aType;
    }

    public void Set(object[] value)
    {
        if (PropertyType != Type.Array) throw new InvalidOperationException("This property is not an array type.");
        this.value.a = new VariablePropertyBase[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            this.value.a[i] = new VariablePropertyBase(aType);
            switch (aType)
            {
                case VariableProperty.Type.Boolean:
                    this.value.a[i].Set((bool)value[i]);
                    break;
                case VariableProperty.Type.Number:
                    this.value.a[i].Set((double)value[i]);
                    break;
                case VariableProperty.Type.String:
                    this.value.a[i].Set((string)value[i]);
                    break;
                case VariableProperty.Type.Object:
                    this.value.a[i].Set((UnityEngine.Object)value[i]);
                    break;
                case VariableProperty.Type.Vector:
                    this.value.a[i].Set((Vector2)value[i]);
                    break;
            }
        }
    }

    public int GetEnum()
    {
        if (PropertyType != Type.Enum) throw new InvalidOperationException("This property is not an enum type.");
        return value.e;
    }

    public T GetEnum<T>() where T : Enum
    {
        if (PropertyType != Type.Enum) throw new InvalidOperationException("This property is not an enum type.");
        return (T)Enum.ToObject(ObjectType, value.e);
    }

    public void Set(int value)
    {
        if (PropertyType != Type.Enum) throw new InvalidOperationException("This property is not an enum type.");
        this.value.e = value;
    }

    public enum Type
    {
        Boolean,
        Number,
        String,
        Object,
        Vector,
        Array,
        Enum
    }

    public enum Display
    {
        Standard,
        Input,
        Output
    }

    [System.Serializable]
    private struct Value
    {
        public bool b;
        public double n;
        public string s;
        public UnityEngine.Object o;
        public Vector2 v;
        public VariablePropertyBase[] a;
        public int e;
    }

    [System.Serializable]
    private class VariablePropertyBase
    {
        [SerializeField] private Type propertyType;
        public Type PropertyType
        {
            get => propertyType;
            private set => propertyType = value;
        }

        [SerializeField] private ValueBase value;
        [SerializeField] private System.Type oType;

        public VariablePropertyBase(Type type)
        {
            PropertyType = type;
            value = new ValueBase();
        }

        public VariablePropertyBase(Type type, System.Type objectType)
        {
            PropertyType = Type.Object;
            oType = objectType;
            value = new ValueBase();
        }

        public bool GetBoolean()
        {
            if (PropertyType != Type.Boolean) throw new InvalidOperationException("This property is not a boolean type.");
            return value.b;
        }

        public void Set(bool value)
        {
            if (PropertyType != Type.Boolean) throw new InvalidOperationException("This property is not a boolean type.");
            this.value.b = value;
        }

        public double GetNumber()
        {
            if (PropertyType != Type.Number) throw new InvalidOperationException("This property is not a number type.");
            return value.n;
        }

        public void Set(double value)
        {
            if (PropertyType != Type.Number) throw new InvalidOperationException("This property is not a number type.");
            this.value.n = value;
        }

        public string GetString()
        {
            if (PropertyType != Type.String) throw new InvalidOperationException("This property is not a string type.");
            return value.s;
        }

        public void Set(string value)
        {
            if (PropertyType != Type.String) throw new InvalidOperationException("This property is not a string type.");
            this.value.s = value;
        }

        public UnityEngine.Object GetObject()
        {
            if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
            return value.o;
        }

        public System.Type GetObjectType()
        {
            if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
            return oType;
        }

        public void Set(UnityEngine.Object value)
        {
            if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
            this.value.o = value;
        }

        public Vector2 GetVector()
        {
            if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not a vector type.");
            return value.v;
        }

        public void Set(Vector2 value)
        {
            if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not a vector type.");
            this.value.v = value;
        }

        [System.Serializable]
        private struct ValueBase
        {
            public bool b;
            public double n;
            public string s;
            public UnityEngine.Object o;
            public Vector2 v;
        }
    }
}