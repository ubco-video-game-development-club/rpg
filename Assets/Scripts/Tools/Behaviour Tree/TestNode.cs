using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TestNode : IBehaviourTreeNode
    {
        public void Init(BehaviourTreeNode self)
        {
            self.AddProperty("Foo", new VariableProperty(VariableProperty.Type.Boolean));
        }

        public NodeStatus Tick(Tree<BehaviourTreeNode>.Node self)
        {
            return NodeStatus.Success;
        }
    }
}
