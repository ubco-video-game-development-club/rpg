using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class EqualsNode : IBehaviourTreeNode
    {
        private const string PROP_SOURCE = "source-property";
        private const string PROP_NUMBER = "comparison-number";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_SOURCE, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_NUMBER, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string source = behaviour.GetProperty(PROP_SOURCE).GetString();
            int num1 = (int)obj.GetProperty(source);
            int num2 = (int)behaviour.GetProperty(PROP_NUMBER).GetNumber();
            if (num1 == num2) return NodeStatus.Success;

            return NodeStatus.Failure;
        }
    }
}