using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class IsNullNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("prop-name", new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            string propName = self.Element.GetProperty("prop-name").GetString();
            return agent.HasProperty(propName) ? NodeStatus.Failure : NodeStatus.Success;
        }
    }
}
