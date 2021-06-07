using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        private Dictionary<PropertyName, dynamic> properties = new Dictionary<PropertyName, dynamic>();
        public Dictionary<PropertyName, dynamic> Properties { get => properties; }

        private Dictionary<PropertyName, UnityEventBase> propertyChangedEvents = new Dictionary<PropertyName, UnityEventBase>();

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
            GetPropertyChangedEvent<T>(name).Invoke(value);
        }

        public void AddPropertyChangedListener<T>(PropertyName name, UnityAction<T> listener)
        {
            GetPropertyChangedEvent<T>(name).AddListener(listener);
        }

        public UnityEvent<T> GetPropertyChangedEvent<T>(PropertyName name)
        {
            if (!propertyChangedEvents.ContainsKey(name))
            {
                propertyChangedEvents[name] = new UnityEvent<T>();
            }
            return (UnityEvent<T>)propertyChangedEvents[name];
        }
    }
}
