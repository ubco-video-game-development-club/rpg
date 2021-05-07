using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Dictionary<PropertyName, dynamic> properties = new Dictionary<PropertyName, dynamic>();

    public T GetProperty<T>(PropertyName name)
    {
        return properties[name];
    }

    public bool TryGetProperty<T>(PropertyName name, out T property)
    {
        bool exists = properties.ContainsKey(name);
        property = exists ? GetProperty<T>(name) : default(T);
        return exists;
    }

    public void SetProperty<T>(PropertyName name, T value)
    {
        properties[name] = value;
    }
}
