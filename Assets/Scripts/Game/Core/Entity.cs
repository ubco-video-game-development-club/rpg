using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public abstract class Entity : MonoBehaviour
    {
        private PropertySet properties = new PropertySet();
        public PropertySet Properties { get => properties; }

        private Dictionary<PropertyName, UnityEventBase> propertyChangedEvents = new Dictionary<PropertyName, UnityEventBase>();

        ///<summary>Returns the Entity matching the given unique id. The desired Entity must have a corresponding UniqueID component.</summary>
        public static Entity Find(string id)
        {
            GameObject obj = UniqueID.Get(id);
            if (obj == null)
            {
                Debug.LogWarning($"Entity.Find() failed to find Entity with ID \"{id}\"; returning null.");
                return null;
            }
            return obj.GetComponent<Entity>();
        }

        public bool HasProperty(PropertyName name)
        {
            return properties.HasProperty(name);
        }

        public T GetProperty<T>(PropertyName name)
        {
            return properties.GetProperty<T>(name);
        }

        public bool TryGetProperty<T>(PropertyName name, out T property)
        {
            property = GetProperty<T>(name);
            return HasProperty(name);
        }

        public void SetProperty<T>(PropertyName name, T value)
        {
            properties.SetProperty<T>(name, value);
            GetPropertyChangedEvent<T>(name).Invoke(value);
        }

        public void AddPropertyChangedListener<T>(PropertyName name, UnityAction<T> listener)
        {
            GetPropertyChangedEvent<T>(name).AddListener(listener);
        }

        public void RemovePropertyChangedListener<T>(PropertyName name, UnityAction<T> listener)
        {
            GetPropertyChangedEvent<T>(name).RemoveListener(listener);
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
