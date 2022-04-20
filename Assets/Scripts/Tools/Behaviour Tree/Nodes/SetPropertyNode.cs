using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetPropertyNode : IBehaviourTreeNode
    {
        private enum ValueType { Int, Double, Bool, String, Vector }

        private const string PROP_TYPE = "property-type";
        private const string PROP_TARGET = "target-property";
        private const string PROP_VALUE = "property-value";

        public void Serialize(Behaviour behaviour)
        {
            if (!behaviour.Properties.ContainsKey(PROP_TYPE))
            {
                VariableProperty propTypeVar = new VariableProperty(VariableProperty.Type.Enum, typeof(ValueType));
                propTypeVar.ForceReserialization = true;
                behaviour.SetProperty(PROP_TYPE, propTypeVar);
            }
            ValueType valueType = behaviour.GetProperty(PROP_TYPE).GetEnum<ValueType>();
            behaviour.SetProperty(PROP_TARGET, new VariableProperty(VariableProperty.Type.String));
            behaviour.SetProperty(PROP_VALUE, new VariableProperty(ToPropertyType(valueType)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string target = behaviour.GetProperty(PROP_TARGET).GetString();
            ValueType valueType = behaviour.GetProperty(PROP_TYPE).GetEnum<ValueType>();
            switch (valueType)
            {
                case ValueType.Int:
                    int iVal = (int)behaviour.GetProperty(PROP_VALUE).GetNumber();
                    obj.SetProperty(target, iVal);
                    break;
                case ValueType.Double:
                    double dVal = behaviour.GetProperty(PROP_VALUE).GetNumber();
                    obj.SetProperty(target, dVal);
                    break;
                case ValueType.Bool:
                    bool bVal = behaviour.GetProperty(PROP_VALUE).GetBoolean();
                    obj.SetProperty(target, bVal);
                    break;
                case ValueType.String:
                    string sVal = behaviour.GetProperty(PROP_VALUE).GetString();
                    obj.SetProperty(target, sVal);
                    break;
                case ValueType.Vector:
                    Vector2 vVal = behaviour.GetProperty(PROP_VALUE).GetVector();
                    obj.SetProperty(target, vVal);
                    break;
            }

            return NodeStatus.Success;
        }

        private VariableProperty.Type ToPropertyType(ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.Int:
                case ValueType.Double:
                    return VariableProperty.Type.Number;
                case ValueType.Bool:
                    return VariableProperty.Type.Boolean;
                case ValueType.String:
                    return VariableProperty.Type.String;
                case ValueType.Vector:
                    return VariableProperty.Type.Vector;
            }
            return VariableProperty.Type.Number;
        }
    }
}
