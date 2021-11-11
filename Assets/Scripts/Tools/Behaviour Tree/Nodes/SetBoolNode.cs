using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetBoolNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET = "target-property";
        private const string PROP_VALUE = "bool-value";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_VALUE, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string target = behaviour.GetProperty(PROP_TARGET).GetString();
            bool val = behaviour.GetProperty(PROP_VALUE).GetBoolean();
            obj.SetProperty(target, val);

            return NodeStatus.Failure;
        }
    }
}
