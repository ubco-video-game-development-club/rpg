using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BoolEqualsNode : IBehaviourTreeNode
    {
        private const string PROP_SOURCE = "source-property";
        private const string PROP_BOOL = "comparison-bool";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_SOURCE, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_BOOL, new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string source = behaviour.GetProperty(PROP_SOURCE).GetString();
            bool bool1 = (bool)obj.GetProperty(source);
            bool bool2 = behaviour.GetProperty(PROP_BOOL).GetBoolean();
            if (bool1 == bool2) return NodeStatus.Success;

            return NodeStatus.Failure;
        }
    }
}
