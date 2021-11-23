using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public enum EntityPropertyReadType { Int, Float, Bool, String } // TODO: replace with RPG.PropertyType (git gud)

    public class GetEntityPropertyNode : IBehaviourTreeNode
    {
        private const string PROP_NAME = "property-name";
        private const string PROP_TYPE = "property-type";
        private const string PROP_DEST = "property-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NAME, new VariableProperty(VariableProperty.Type.Enum, typeof(RPG.PropertyName)));
            behaviour.Properties.Add(PROP_TYPE, new VariableProperty(VariableProperty.Type.Enum, typeof(EntityPropertyReadType)));
            behaviour.Properties.Add(PROP_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            Debug.Log("CHECKING HEALTHHHHH");

            Entity entity = null;
            if (!obj.TryGetComponent<Entity>(out entity))
            {
                Debug.LogError("Attempted to use GetEntityPropertyNode with no Entity component!");
                return NodeStatus.Failure;
            }

            RPG.PropertyName propName = behaviour.GetProperty(PROP_NAME).GetEnum<RPG.PropertyName>();
            Debug.Log(propName);
            foreach (var p in entity.Properties)
            {
                Debug.Log(p.Key);
                Debug.Log(p.Value);
            }
            if (!entity.HasProperty(propName))
            {
                Debug.Log("PROP NOT FOUND!!");
                return NodeStatus.Failure;
            }

            EntityPropertyReadType readType = behaviour.GetProperty(PROP_TYPE).GetEnum<EntityPropertyReadType>();
            string propDest = behaviour.GetProperty(PROP_DEST).GetString();
            switch (readType)
            {
                case EntityPropertyReadType.Int:
                    int iVal = entity.GetProperty<int>(propName);
                    obj.SetProperty(propDest, iVal);
                    Debug.Log("READING INT VALUE: " + iVal);
                    break;
                case EntityPropertyReadType.Float:
                    float fVal = entity.GetProperty<float>(propName);
                    obj.SetProperty(propDest, fVal);
                    break;
                case EntityPropertyReadType.Bool:
                    bool bVal = entity.GetProperty<bool>(propName);
                    obj.SetProperty(propDest, bVal);
                    break;
                case EntityPropertyReadType.String:
                    string sVal = entity.GetProperty<string>(propName);
                    obj.SetProperty(propDest, sVal);
                    break;
            }

            Debug.Log("READ THAT SHIII: " + obj.GetProperty(propDest).ToString());

            return NodeStatus.Success;
        }
    }
}
