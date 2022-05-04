using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetActionTargetNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_SRC = "target-source";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_SRC, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.GetProperty(instance, PROP_TARGET_SRC).GetString();
            Vector2 target = (Vector2)obj.GetProperty(src);
            obj.GetComponent<Enemy>().SetActionTarget(target);

            return NodeStatus.Success;
        }
    }
}