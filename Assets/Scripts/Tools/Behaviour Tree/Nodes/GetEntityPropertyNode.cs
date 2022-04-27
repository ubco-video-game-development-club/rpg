using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class GetEntityPropertyNode : IBehaviourTreeNode
    {
        private const string PROP_NAME = "property-name";
        private const string PROP_TYPE = "property-type";
        private const string PROP_DEST = "property-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NAME, new VariableProperty(VariableProperty.Type.Enum, typeof(RPG.PropertyName)));
            behaviour.Properties.Add(PROP_TYPE, new VariableProperty(VariableProperty.Type.Enum, typeof(PropertyType)));
            behaviour.Properties.Add(PROP_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            Entity entity = null;
            if (!obj.TryGetComponent<Entity>(out entity))
            {
                return NodeStatus.Failure;
            }

            RPG.PropertyName propName = behaviour.GetProperty(PROP_NAME).GetEnum<RPG.PropertyName>();
            if (!entity.HasProperty(propName))
            {
                Debug.LogWarning("Warning: target entity " + entity.name + " does not have property " + propName);
                return NodeStatus.Failure;
            }

            PropertyType readType = behaviour.GetProperty(PROP_TYPE).GetEnum<PropertyType>();
            string propDest = behaviour.GetProperty(PROP_DEST).GetString();
            switch (readType)
            {
                case PropertyType.Int:
                    int iVal = entity.GetProperty<int>(propName);
                    obj.SetProperty(propDest, iVal);
                    break;
                case PropertyType.Float:
                    float fVal = entity.GetProperty<float>(propName);
                    obj.SetProperty(propDest, fVal);
                    break;
                case PropertyType.Bool:
                    bool bVal = entity.GetProperty<bool>(propName);
                    obj.SetProperty(propDest, bVal);
                    break;
                case PropertyType.String:
                    string sVal = entity.GetProperty<string>(propName);
                    obj.SetProperty(propDest, sVal);
                    break;
            }

            return NodeStatus.Success;
        }
    }
}
