using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourTreeNodeType
    {
        FindObjectByTagNode,
        GetActorPositionNode,
        SelectorNode,
        SequenceNode,
    }

    public static class BehaviourTreeNodeCreator
    {
        public static IBehaviourTreeNode Create(BehaviourTreeNodeType type)
        {
            switch (type)
            {
                case BehaviourTreeNodeType.FindObjectByTagNode:
                    return new FindObjectByTagNode();
                case BehaviourTreeNodeType.GetActorPositionNode:
                    return new GetActorPositionNode();
                case BehaviourTreeNodeType.SelectorNode:
                    return new SelectorNode();
                case BehaviourTreeNodeType.SequenceNode:
                    return new SequenceNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
