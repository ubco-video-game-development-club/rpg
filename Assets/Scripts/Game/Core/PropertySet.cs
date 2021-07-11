using System.Collections.Generic;

namespace RPG
{
    public class PropertySet : Dictionary<PropertyName, dynamic>
    {
        public bool HasProperty(PropertyName name)
        {
            return ContainsKey(name);
        }

        public T GetProperty<T>(PropertyName name)
        {
            return HasProperty(name) ? this[name] : default(T);
        }

        public void SetProperty<T>(PropertyName name, T value)
        {
            this[name] = value;
        }
    }
}
