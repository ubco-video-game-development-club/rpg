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

    public VariableProperty(Type type)
    {
        PropertyType = type;
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

    public enum Type
    {
        Boolean,
        Number,
        String
    }

    [System.Serializable]
    private struct Value
    {
        public bool b;
        public double n;
        public string s;
    }
}