using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GetRandomPositionNode : IBehaviourTreeNode
    {
        private const string PROP_ORIGIN_POSITION = "origin-position";
        private const string PROP_MAX_DIST = "max-distance";
        private const string PROP_POSITION_DEST = "position-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_ORIGIN_POSITION, new VariableProperty(VariableProperty.Type.Vector));
            behaviour.Properties.Add(PROP_MAX_DIST, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_POSITION_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string dest = behaviour.GetProperty(PROP_POSITION_DEST).GetString();
            Vector2 origin = behaviour.GetProperty(PROP_ORIGIN_POSITION).GetVector();
            float maxDist = (float)behaviour.GetProperty(PROP_MAX_DIST).GetNumber();
            obj.SetProperty(dest, origin + Random.insideUnitCircle * maxDist);

            return NodeStatus.Success;
        }
    }
}