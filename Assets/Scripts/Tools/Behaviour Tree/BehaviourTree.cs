using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Behaviour Tree", order = 65)]
    public class BehaviourTree : ScriptableObject
    {
        public Tree<IBehaviourTreeNode> node;
    }

    public enum NodeStatus
    {
        Success,
        Running,
        Failure
    }

    public interface IBehaviourTreeNode
    {
        NodeStatus Tick();
        void AddProperty(VariableProperty property);
        VariableProperty[] GetProperties();
        void RemoveProperty(VariableProperty property);
    }
}