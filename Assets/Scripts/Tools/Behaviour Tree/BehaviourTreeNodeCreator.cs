using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourTreeNodeType
    {
        SelectorNode,
        SequenceNode,
        SuccessNode,
        NotNode,
        EqualsNode,
        HasPropertyNode,
        RangeCheckNode,
        FindActorByTagNode,
        GetActorPositionNode,
        GetRandomPositionNode,
        SimpleMoveNode,
        GetDialogueIndexNode,
        SetDialogueIndexNode
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
                case BehaviourTreeNodeType.SuccessNode:
                    return new SuccessNode();
                case BehaviourTreeNodeType.NotNode:
                    return new NotNode();
                case BehaviourTreeNodeType.EqualsNode:
                    return new EqualsNode();
                case BehaviourTreeNodeType.HasPropertyNode:
                    return new HasPropertyNode();
                case BehaviourTreeNodeType.RangeCheckNode:
                    return new RangeCheckNode();
                case BehaviourTreeNodeType.FindActorByTagNode:
                    return new FindActorByTagNode();
                case BehaviourTreeNodeType.GetActorPositionNode:
                    return new GetActorPositionNode();
                case BehaviourTreeNodeType.GetRandomPositionNode:
                    return new GetRandomPositionNode();
                case BehaviourTreeNodeType.SimpleMoveNode:
                    return new SimpleMoveNode();
                case BehaviourTreeNodeType.GetDialogueIndexNode:
                    return new GetDialogueIndexNode();
                case BehaviourTreeNodeType.SetDialogueIndexNode:
                    return new SetDialogueIndexNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
