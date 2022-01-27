using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ArrayIteratorNode : IBehaviourTreeNode
    {
        private enum ValueType { Int, Double, Bool, String, Vector }

        private const string PROP_TYPE = "property-type";
        private const string PROP_ARRAY = "value-array";
        private const string PROP_INDEX_SRC = "index-source";
        private const string PROP_VALUE_DEST = "value-destination";

        public void Serialize(Behaviour behaviour)
        {
            if (!behaviour.Properties.ContainsKey(PROP_TYPE))
            {
                behaviour.SetProperty(PROP_TYPE, new VariableProperty(VariableProperty.Type.Enum, typeof(ValueType), true));
            }
            ValueType valueType = behaviour.GetProperty(PROP_TYPE).GetEnum<ValueType>();
            behaviour.SetProperty(PROP_ARRAY, new VariableProperty(VariableProperty.Type.Array, ToPropertyType(valueType)));
            behaviour.SetProperty(PROP_INDEX_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.SetProperty(PROP_VALUE_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            // read the current index from the agent (or set to zero if not exists)
            int index = 0;
            string indexSrc = behaviour.GetProperty(PROP_INDEX_SRC).GetString();
            if (obj.HasProperty(indexSrc))
            {
                index = (int)obj.GetProperty(indexSrc);
            }

            // get the value at the current index
            object[] arr = behaviour.GetProperty(PROP_ARRAY).GetArray();
            object val = null;
            ValueType valueType = behaviour.GetProperty(PROP_TYPE).GetEnum<ValueType>();
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
            string valueDest = behaviour.GetProperty(PROP_VALUE_DEST).GetString();
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
