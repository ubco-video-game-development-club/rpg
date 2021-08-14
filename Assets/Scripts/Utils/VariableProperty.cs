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

    /*public VariableProperty[] GetArray()
    {
        if(PropertyType != Type.Array) throw new InvalidOperationException("This property is not an array type.");
        return value.a;
    }*/

    /*public void Set(VariableProperty[] value)
    {
        if (PropertyType != Type.Array) throw new InvalidOperationException("This property is not an array type.");
        this.value.a = value;
    }*/

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
        // Array,
        String
    }

    /*
        NOTICE:
        The explicit struct layout was causing Unity to crash on serialization.
        Honestly that's not a surprise.
        Thus, this type will take up slightly more memory, but it's not a big deal.
    */

    //[StructLayout(LayoutKind.Explicit)] //Explicit struct layout allows positions in memory to be defined
    [System.Serializable]
    private struct Value
    {
        //Making field offset zero essentially creates a C-style union
        /*[FieldOffset(0)]*/
        public bool b;
        /*[FieldOffset(0)]*/
        public double n;
        //[FieldOffset(0)] public VariableProperty[] a;
        /*[FieldOffset(0)]*/
        public string s;
    }
}