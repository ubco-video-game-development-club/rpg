using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class IntLessThanNode : IBehaviourTreeNode
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

            Debug.Log("CHECKING LESS THAN");

            string source = behaviour.GetProperty(PROP_SOURCE).GetString();
            int num1 = (int)obj.GetProperty(source);
            int num2 = (int)behaviour.GetProperty(PROP_NUMBER).GetNumber();
            if (num1 < num2) return NodeStatus.Success;

            Debug.Log("NOT LESS THAN");

            return NodeStatus.Failure;
        }
    }
}
