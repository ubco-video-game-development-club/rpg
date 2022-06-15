using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class SetActionTargetNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_INPUT = "target-input";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_TARGET_INPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string src = behaviour.GetProperty(instance, PROP_TARGET_INPUT).GetString();
            Vector2 target = (Vector2)obj.GetProperty(src);
            obj.GetComponent<Enemy>().SetActionTarget(target);

            return NodeStatus.Success;
        }
    }
}