using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourTreeNodeType
    {
        SelectorNode,
        SequenceNode,
        TestMoverNode,
    }

    public static class BehaviourTreeNodeCreator
    {
        public static IBehaviourTreeNode Create(BehaviourTreeNodeType type)
        {
            switch(type)
            {
                case BehaviourTreeNodeType.SelectorNode:
                    return new SelectorNode();
                case BehaviourTreeNodeType.SequenceNode:
                    return new SequenceNode();
                case BehaviourTreeNodeType.TestMoverNode:
                    return new TestMoverNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}