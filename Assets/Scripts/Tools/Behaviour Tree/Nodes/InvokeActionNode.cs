using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class InvokeActionNode : IBehaviourTreeNode
    {
        private const string PROP_ACTION_IDX = "action-index";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_ACTION_IDX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            int idx = (int)behaviour.GetProperty(instance, PROP_ACTION_IDX).GetNumber();
            bool success = obj.GetComponent<Enemy>().InvokeAction(idx);
            return success ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}