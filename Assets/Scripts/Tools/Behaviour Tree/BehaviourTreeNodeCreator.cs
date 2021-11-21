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
        IntEqualsNode,
        BoolEqualsNode,
        HasPropertyNode,
        RangeCheckNode,
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
        VectorIteratorNode,
        ArrayIncrementNode,
        SetBoolNode,
        GetMoveDirectionNode,
        GetVectorAxisNode,
        IsEnemyNode,
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
                case BehaviourTreeNodeType.IntEqualsNode:
                    return new IntEqualsNode();
                case BehaviourTreeNodeType.BoolEqualsNode:
                    return new BoolEqualsNode();
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
                case BehaviourTreeNodeType.VectorIteratorNode:
                    return new VectorIteratorNode();
                case BehaviourTreeNodeType.ArrayIncrementNode:
                    return new ArrayIncrementNode();
                case BehaviourTreeNodeType.SetBoolNode:
                    return new SetBoolNode();
                case BehaviourTreeNodeType.GetMoveDirectionNode:
                    return new GetMoveDirectionNode();
                case BehaviourTreeNodeType.GetVectorAxisNode:
                    return new GetVectorAxisNode();
                case BehaviourTreeNodeType.IsEnemyNode:
                    return new IsEnemyNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }
    }
}
