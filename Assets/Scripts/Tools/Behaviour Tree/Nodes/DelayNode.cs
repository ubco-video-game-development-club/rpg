using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class DelayNode : IBehaviourTreeNode
    {
        private const string PROP_DELAY_DURATION = "delay-duration";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_DELAY_DURATION, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            obj.StartCoroutine(TickDelayed(self, obj, instance));
            return NodeStatus.Success;
        }

        private IEnumerator TickDelayed(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            float delayDuration = (float)behaviour.GetProperty(instance, PROP_DELAY_DURATION).GetNumber();

            yield return new WaitForSeconds(delayDuration);

            for (int i = 0; i < self.ChildCount; i++)
            {
                Tree<Behaviour>.Node child = self.GetChild(i);
                child.Element.Tick(child, obj, instance);
            }
        }
    }
}
