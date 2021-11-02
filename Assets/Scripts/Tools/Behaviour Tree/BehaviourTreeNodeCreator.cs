using UnityEngine;

namespace BehaviourTree
{
    public enum BehaviourTreeNodeType
    {
        SelectorNode,
        SequenceNode,
        ExecuteAllNode,
        SuccessNode,
        NotNode,
        EqualsNode,
        HasPropertyNode,
        RangeCheckNode,
        FindActorByTagNode,
        FindObjectByIdNode,
        GetActorPositionNode,
        GetRandomPositionNode,
        SimpleMoveNode,
        SetActionTargetNode,
        InvokeActionNode,
        GetDialogueIndexNode,
        SetDialogueIndexNode,
        GetMoveDirectionNode,
        GetVectorAxisNode
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
                case BehaviourTreeNodeType.ExecuteAllNode:
                    return new ExecuteAllNode();
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
                case BehaviourTreeNodeType.FindObjectByIdNode:
                    return new FindObjectByIdNode();
                case BehaviourTreeNodeType.GetActorPositionNode:
                    return new GetActorPositionNode();
                case BehaviourTreeNodeType.GetRandomPositionNode:
                    return new GetRandomPositionNode();
                case BehaviourTreeNodeType.SimpleMoveNode:
                    return new SimpleMoveNode();
                case BehaviourTreeNodeType.SetActionTargetNode:
                    return new SetActionTargetNode();
                case BehaviourTreeNodeType.InvokeActionNode:
                    return new InvokeActionNode();
                case BehaviourTreeNodeType.GetDialogueIndexNode:
                    return new GetDialogueIndexNode();
                case BehaviourTreeNodeType.SetDialogueIndexNode:
                    return new SetDialogueIndexNode();
                case BehaviourTreeNodeType.GetMoveDirectionNode:
                    return new GetMoveDirectionNode();
                case BehaviourTreeNodeType.GetVectorAxisNode:
                    return new GetVectorAxisNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
