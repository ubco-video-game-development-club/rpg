using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class SetActionTargetNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_SRC = "target-source";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.Properties[PROP_TARGET_SRC].GetString();
            Vector2 target = (Vector2)obj.GetProperty(src);
            obj.GetComponent<Enemy>().SetActionTarget(target);

            Debug.Log(obj.name + " is attacking position " + target);

            return NodeStatus.Success;
        }
    }
}