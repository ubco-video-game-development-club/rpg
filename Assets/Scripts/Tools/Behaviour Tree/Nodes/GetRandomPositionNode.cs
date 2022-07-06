using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class GetRandomPositionNode : IBehaviourTreeNode
    {
        private const string PROP_ORIGIN_POSITION = "origin-position";
        private const string PROP_MAX_DIST = "max-distance";
        private const string PROP_POSITION_OUTPUT = "position-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddProperty(PROP_ORIGIN_POSITION, new VariableProperty(VariableProperty.Type.Vector));
            behaviour.AddProperty(PROP_MAX_DIST, new VariableProperty(VariableProperty.Type.Number));
            behaviour.AddOutputProperty(PROP_POSITION_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string dest = behaviour.GetProperty(instance, PROP_POSITION_OUTPUT).GetString();
            Vector2 origin = behaviour.GetProperty(instance, PROP_ORIGIN_POSITION).GetVector();
            float maxDist = (float)behaviour.GetProperty(instance, PROP_MAX_DIST).GetNumber();
            obj.SetProperty(dest, origin + Random.insideUnitCircle * maxDist);

            return NodeStatus.Success;
        }
    }
}