using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourTreeNodeType
    {
        SelectorNode,
        SequenceNode,
        GetPlayerNode,
        AttackNode,
        MoveNode
    }

    public static class BehaviourTreeNodeCreator
    {
        public static IBehaviourTreeNode Create(BehaviourTreeNodeType type)
        {
            switch (type)
            {
                case BehaviourTreeNodeType.SelectorNode:
                    return new SelectorNode();
                case BehaviourTreeNodeType.SequenceNode:
                    return new SequenceNode();
                case BehaviourTreeNodeType.GetPlayerNode:
                    return new GetPlayerNode();
                case BehaviourTreeNodeType.AttackNode:
                    return new AttackNode();
                case BehaviourTreeNodeType.MoveNode:
                    return new MoveNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
