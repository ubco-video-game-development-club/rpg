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

    [SerializeField] private Value value;
    [SerializeField] private System.Type oType;

    public VariableProperty(Type type)
    {
        PropertyType = type;
        value = new Value();
    }

    public VariableProperty(System.Type objectType)
    {
        PropertyType = Type.Object;
        oType = objectType;
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

    public System.Type GetObjectType()
    {
        if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not an object type.");
        return oType;
    }

    public void Set(UnityEngine.Object value)
    {
        if (PropertyType != Type.Object) throw new InvalidOperationException("This property is not a object type.");
        this.value.o = value;
    }

    public Vector2 GetVector()
    {
        if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not an vector type.");
        return value.v;
    }

    public void Set(Vector2 value)
    {
        if (PropertyType != Type.Vector) throw new InvalidOperationException("This property is not a vector type.");
        this.value.v = value;
    }

    public enum Type
    {
        Boolean,
        Number,
        String,
        Object,
        Vector
    }

    [System.Serializable]
    private struct Value
    {
        public bool b;
        public double n;
        public string s;
        public UnityEngine.Object o;
        public Vector2 v;
    }
}