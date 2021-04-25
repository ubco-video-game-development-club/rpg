using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TestNode : BehaviourTreeNode
    {
        public TestNode()
        {
            AddProperty(new VariableProperty("Foo", VariableProperty.Type.Boolean));
        }

        public override NodeStatus Tick(Tree<BehaviourTreeNode>.Node self)
        {
            return NodeStatus.Success;
        }
    }
}
