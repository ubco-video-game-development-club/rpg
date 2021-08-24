using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class GetRandomDestinationNode : IBehaviourTreeNode
    {
        public void Init(Behaviour behaviour)
        {
            behaviour.SetProperty("origin", new VariableProperty(VariableProperty.Type.Vector));
            behaviour.SetProperty("max-distance", new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, Agent agent)
        {
            Vector2 origin = self.Element.GetProperty("origin").GetVector();
            float maxDist = (float)self.Element.GetProperty("max-distance").GetNumber();
            Vector2 randPos = origin + Random.insideUnitCircle * maxDist;
            agent.SetProperty("destination", randPos);
            return NodeStatus.Success;
        }
    }
}
