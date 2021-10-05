using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourObject : MonoBehaviour
{
    public Dictionary<string, object> Properties { get; private set; }

    protected virtual void Awake()
    {
        Properties = new Dictionary<string, object>();
    }

    public void SetProperty(string name, object property)
    {
        Properties[name] = property;
    }

    public object GetProperty(string name)
    {
        return HasProperty(name) ? Properties[name] : null;
    }

    public bool HasProperty(string name)
    {
        return Properties.ContainsKey(name);
    }

    public void RemoveProperty(string name)
    {
        Properties.Remove(name);
    }
}
