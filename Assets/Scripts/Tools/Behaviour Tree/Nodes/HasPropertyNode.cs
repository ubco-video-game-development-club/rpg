using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class HasPropertyNode : IBehaviourTreeNode
    {
        private const string PROP_NAME = "property-name";

        public void Init(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_NAME, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.GetProperty(PROP_NAME).GetString();
            if (agent.HasProperty(src))
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}