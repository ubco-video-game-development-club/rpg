using UnityEngine;

namespace Behaviours
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
        MoveToNode,
        MoveFollowNode,
        IdleNode,
        DelayNode,
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
        SetEnemyGroupNode,
        IsEnemyNode,
        GetEntityPropertyNode,
        SubTreeNode,
        SpawnPlayerNode,
        GetLevelPropertyNode,
        SetLevelPropertyNode,
    }

    public enum BehaviourTreeNodeCategory
    {
        Basic,
        Logic,
        Property,
        Find,
        Timing,
        Movement,
        Combat,
        Dialogue,
        Player
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
                case BehaviourTreeNodeType.MoveToNode:
                    return new MoveToNode();
                case BehaviourTreeNodeType.MoveFollowNode:
                    return new MoveFollowNode();
                case BehaviourTreeNodeType.IdleNode:
                    return new IdleNode();
                case BehaviourTreeNodeType.DelayNode:
                    return new DelayNode();
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
                case BehaviourTreeNodeType.SetEnemyGroupNode:
                    return new SetEnemyGroupNode();
                case BehaviourTreeNodeType.IsEnemyNode:
                    return new IsEnemyNode();
                case BehaviourTreeNodeType.GetEntityPropertyNode:
                    return new GetEntityPropertyNode();
                case BehaviourTreeNodeType.SubTreeNode:
                    return new SubTreeNode();
                case BehaviourTreeNodeType.SpawnPlayerNode:
                    return new SpawnPlayerNode();
                case BehaviourTreeNodeType.GetLevelPropertyNode:
                    return new GetLevelPropertyNode();
                case BehaviourTreeNodeType.SetLevelPropertyNode:
                    return new SetLevelPropertyNode();
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return null;
            }
        }

        public static BehaviourTreeNodeCategory GetCategory(BehaviourTreeNodeType type)
        {
            switch (type)
            {
                case BehaviourTreeNodeType.SelectorNode:
                case BehaviourTreeNodeType.SequenceNode:
                case BehaviourTreeNodeType.ExecuteAllNode:
                case BehaviourTreeNodeType.SubTreeNode:
                    return BehaviourTreeNodeCategory.Basic;
                case BehaviourTreeNodeType.SuccessNode:
                case BehaviourTreeNodeType.NotNode:
                case BehaviourTreeNodeType.CompareNode:
                case BehaviourTreeNodeType.RangeCheckNode:
                    return BehaviourTreeNodeCategory.Logic;
                case BehaviourTreeNodeType.HasPropertyNode:
                case BehaviourTreeNodeType.SetPropertyNode:
                case BehaviourTreeNodeType.ArrayIteratorNode:
                case BehaviourTreeNodeType.ArrayIncrementNode:
                case BehaviourTreeNodeType.GetEntityPropertyNode:
                case BehaviourTreeNodeType.GetLevelPropertyNode:
                case BehaviourTreeNodeType.SetLevelPropertyNode:
                    return BehaviourTreeNodeCategory.Property;
                case BehaviourTreeNodeType.GetSelfNode:
                case BehaviourTreeNodeType.FindActorByTagNode:
                case BehaviourTreeNodeType.FindObjectByIdNode:
                    return BehaviourTreeNodeCategory.Find;
                case BehaviourTreeNodeType.IdleNode:
                case BehaviourTreeNodeType.DelayNode:
                    return BehaviourTreeNodeCategory.Timing;
                case BehaviourTreeNodeType.GetActorPositionNode:
                case BehaviourTreeNodeType.GetRandomPositionNode:
                case BehaviourTreeNodeType.SimpleMoveNode:
                case BehaviourTreeNodeType.MoveToNode:
                case BehaviourTreeNodeType.MoveFollowNode:
                case BehaviourTreeNodeType.GetMoveDirectionNode:
                case BehaviourTreeNodeType.GetVectorAxisNode:
                    return BehaviourTreeNodeCategory.Movement;
                case BehaviourTreeNodeType.SetActionTargetNode:
                case BehaviourTreeNodeType.InvokeActionNode:
                case BehaviourTreeNodeType.SetEnemyNode:
                case BehaviourTreeNodeType.SetEnemyGroupNode:
                case BehaviourTreeNodeType.IsEnemyNode:
                    return BehaviourTreeNodeCategory.Combat;
                case BehaviourTreeNodeType.GetDialogueIndexNode:
                case BehaviourTreeNodeType.SetDialogueIndexNode:
                case BehaviourTreeNodeType.ForceDialogueNode:
                    return BehaviourTreeNodeCategory.Dialogue;
                case BehaviourTreeNodeType.SpawnPlayerNode:
                    return BehaviourTreeNodeCategory.Player;
                default:
                    Debug.LogError($"Unimplemented node type: {type}");
                    return BehaviourTreeNodeCategory.Basic;
            }
        }
    }
}
