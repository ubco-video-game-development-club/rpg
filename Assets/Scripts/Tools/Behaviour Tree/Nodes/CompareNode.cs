using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class CompareNode : IBehaviourTreeNode
    {
        private enum CompareType { Int, Double, Bool, String, Vector }
        private enum OperatorType { Equals, LessThan, GreaterThan }

        private const string PROP_INPUT = "property-input";
        private const string PROP_TYPE = "property-type";
        private const string PROP_OPERATOR = "operator";
        private const string PROP_COMPARATOR = "comparator";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_INPUT);

            if (!behaviour.HasProperty(PROP_TYPE))
            {
                VariableProperty propTypeVar = new VariableProperty(VariableProperty.Type.Enum, typeof(CompareType));
                propTypeVar.ForceReserialization = true;
                behaviour.AddProperty(PROP_TYPE, propTypeVar);
            }
            CompareType comparatorType = behaviour.GetProperty(null, PROP_TYPE).GetEnum<CompareType>();
            behaviour.AddProperty(PROP_OPERATOR, new VariableProperty(VariableProperty.Type.Enum, typeof(OperatorType)));
            behaviour.AddProperty(PROP_COMPARATOR, new VariableProperty(ToPropertyType(comparatorType)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string source = behaviour.GetProperty(instance, PROP_INPUT).GetString();
            CompareType compareType = behaviour.GetProperty(instance, PROP_TYPE).GetEnum<CompareType>();
            OperatorType operatorType = behaviour.GetProperty(instance, PROP_OPERATOR).GetEnum<OperatorType>();

            bool success = false;
            switch (compareType)
            {
                case CompareType.Int:
                    int i1 = (int)obj.GetProperty(source);
                    int i2 = (int)behaviour.GetProperty(instance, PROP_COMPARATOR).GetNumber();
                    success = Compare<int>(i1, i2, operatorType);
                    break;
                case CompareType.Double:
                    double d1 = (double)obj.GetProperty(source);
                    double d2 = behaviour.GetProperty(instance, PROP_COMPARATOR).GetNumber();
                    success = Compare<double>(d1, d2, operatorType);
                    break;
                case CompareType.Bool:
                    bool b1 = (bool)obj.GetProperty(source);
                    bool b2 = behaviour.GetProperty(instance, PROP_COMPARATOR).GetBoolean();
                    success = Compare<bool>(b1, b2, operatorType);
                    break;
                case CompareType.String:
                    string s1 = (string)obj.GetProperty(source);
                    string s2 = behaviour.GetProperty(instance, PROP_COMPARATOR).GetString();
                    success = Compare<string>(s1, s2, operatorType);
                    break;
                case CompareType.Vector:
                    Vector2 v1 = (Vector2)obj.GetProperty(source);
                    Vector2 v2 = behaviour.GetProperty(instance, PROP_COMPARATOR).GetVector();
                    success = Compare<float>(v1.sqrMagnitude, v2.sqrMagnitude, operatorType);
                    break;
            }
            return success ? NodeStatus.Success : NodeStatus.Failure;
        }

        private VariableProperty.Type ToPropertyType(CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.Int:
                case CompareType.Double:
                    return VariableProperty.Type.Number;
                case CompareType.Bool:
                    return VariableProperty.Type.Boolean;
                case CompareType.String:
                    return VariableProperty.Type.String;
                case CompareType.Vector:
                    return VariableProperty.Type.Vector;
            }
            return VariableProperty.Type.Number;
        }

        private bool Compare<T>(T a, T b, OperatorType operation) where T : System.IComparable
        {
            switch (operation)
            {
                case OperatorType.Equals:
                    return a.CompareTo(b) == 0;
                case OperatorType.LessThan:
                    return a.CompareTo(b) < 0;
                case OperatorType.GreaterThan:
                    return a.CompareTo(b) > 0;
            }
            return false;
        }
    }
}
