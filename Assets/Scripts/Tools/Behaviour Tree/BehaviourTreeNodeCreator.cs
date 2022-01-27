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
        CompareNode,
        HasPropertyNode,
        SetPropertyNode,
        RangeCheckNode,
        GetSelfNode,
        FindActorByTagNode,
        FindObjectByIdNode,
        GetActorPositionNode,
        GetRandomPositionNode,
        SimpleMoveNode,
        IdleNode,
        SetActionTargetNode,
        InvokeActionNode,
        GetDialogueIndexNode,
        SetDialogueIndexNode,
        ForceDialogueNode,
        ArrayIteratorNode,
        ArrayIncrementNode,
        GetMoveDirectionNode,
        GetVectorAxisNode,
        SetEnemyNode,
        IsEnemyNode,
        GetEntityPropertyNode,
        SubTreeNode
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
                case BehaviourTreeNodeType.CompareNode:
                    return new CompareNode();
                case BehaviourTreeNodeType.HasPropertyNode:
                    return new HasPropertyNode();
                case BehaviourTreeNodeType.SetPropertyNode:
                    return new SetPropertyNode();
                case BehaviourTreeNodeType.RangeCheckNode:
                    return new RangeCheckNode();
                case BehaviourTreeNodeType.GetSelfNode:
                    return new GetSelfNode();
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
                case BehaviourTreeNodeType.IdleNode:
                    return new IdleNode();
                case BehaviourTreeNodeType.SetActionTargetNode:
                    return new SetActionTargetNode();
                case BehaviourTreeNodeType.InvokeActionNode:
                    return new InvokeActionNode();
                case BehaviourTreeNodeType.GetDialogueIndexNode:
                    return new GetDialogueIndexNode();
                case BehaviourTreeNodeType.SetDialogueIndexNode:
                    return new SetDialogueIndexNode();
                case BehaviourTreeNodeType.ForceDialogueNode:
                    return new ForceDialogueNode();
                case BehaviourTreeNodeType.ArrayIteratorNode:
                    return new ArrayIteratorNode();
                case BehaviourTreeNodeType.ArrayIncrementNode:
                    return new ArrayIncrementNode();
                case BehaviourTreeNodeType.GetMoveDirectionNode:
                    return new GetMoveDirectionNode();
                case BehaviourTreeNodeType.GetVectorAxisNode:
                    return new GetVectorAxisNode();
                case BehaviourTreeNodeType.SetEnemyNode:
                    return new SetEnemyNode();
                case BehaviourTreeNodeType.IsEnemyNode:
                    return new IsEnemyNode();
                case BehaviourTreeNodeType.GetEntityPropertyNode:
                    return new GetEntityPropertyNode();
                case BehaviourTreeNodeType.SubTreeNode:
                    return new SubTreeNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
