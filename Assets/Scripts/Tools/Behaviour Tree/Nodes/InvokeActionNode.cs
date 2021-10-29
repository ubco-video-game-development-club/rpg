using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class InvokeActionNode : IBehaviourTreeNode
    {
        private const string PROP_ACTION_IDX = "action-index";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_ACTION_IDX, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            int idx = (int)behaviour.Properties[PROP_ACTION_IDX].GetNumber();
            obj.GetComponent<Enemy>().InvokeAction(idx);

            return NodeStatus.Success;
        }
    }
}