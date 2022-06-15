using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class ArrayIteratorNode : IBehaviourTreeNode
    {
        private enum ValueType { Int, Double, Bool, String, Vector }

        private const string PROP_INDEX_INPUT = "index-input";
        private const string PROP_TYPE = "property-type";
        private const string PROP_ARRAY = "value-array";
        private const string PROP_VALUE_OUTPUT = "value-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_INDEX_INPUT);

            if (!behaviour.HasProperty(PROP_TYPE))
            {
                VariableProperty propTypeVar = new VariableProperty(VariableProperty.Type.Enum, typeof(ValueType));
                propTypeVar.ForceReserialization = true;
                behaviour.AddProperty(PROP_TYPE, propTypeVar);
            }
            ValueType valueType = behaviour.GetProperty(null, PROP_TYPE).GetEnum<ValueType>();
            behaviour.AddProperty(PROP_ARRAY, new VariableProperty(VariableProperty.Type.Array, ToPropertyType(valueType)));

            behaviour.AddOutputProperty(PROP_VALUE_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            // read the current index from the agent (or set to zero if not exists)
            int index = 0;
            string indexSrc = behaviour.GetProperty(instance, PROP_INDEX_INPUT).GetString();
            if (obj.HasProperty(indexSrc))
            {
                index = (int)obj.GetProperty(indexSrc);
            }

            // get the value at the current index
            object[] arr = behaviour.GetProperty(instance, PROP_ARRAY).GetArray();
            object val = null;
            ValueType valueType = behaviour.GetProperty(instance, PROP_TYPE).GetEnum<ValueType>();
            switch (valueType)
            {
                case ValueType.Int:
                    val = (int)arr[index];
                    break;
                case ValueType.Double:
                    val = (double)arr[index];
                    break;
                case ValueType.Bool:
                    val = (bool)arr[index];
                    break;
                case ValueType.String:
                    val = (string)arr[index];
                    break;
                case ValueType.Vector:
                    val = (Vector2)arr[index];
                    break;
            }

            // store the resulting value on the agent
            string valueDest = behaviour.GetProperty(instance, PROP_VALUE_OUTPUT).GetString();
            obj.SetProperty(valueDest, val);

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
