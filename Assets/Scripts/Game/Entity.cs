using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Dictionary<PropertyName, Object> properties;

    public T GetProperty<T>(PropertyName name) where T : Object
    {
        return properties[name] as T;
    }

    public bool TryGetProperty<T>(PropertyName name, out T property) where T : Object
    {
        bool exists = properties.ContainsKey(name);
        property = exists ? GetProperty<T>(name) : null;
        return exists;
    }

    public void SetProperty<T>(PropertyName name, T value) where T : Object
    {
        properties[name] = value;
    }
}
