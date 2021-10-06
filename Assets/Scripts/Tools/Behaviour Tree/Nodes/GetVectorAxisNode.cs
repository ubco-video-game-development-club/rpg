using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GetVectorAxisNode : IBehaviourTreeNode
    {
        private const string PROP_VECTOR_SRC = "vector-source";
        private const string PROP_AXIS_NAME = "axis-name"; // TODO: make enum
        private const string PROP_AXIS_DEST = "axis-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_VECTOR_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_AXIS_NAME, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_AXIS_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;

            string vecSrc = behaviour.GetProperty(PROP_VECTOR_SRC).GetString();
            Vector2 vec = (Vector2)obj.GetProperty(vecSrc);

            float axis = 0;
            string axisName = behaviour.GetProperty(PROP_AXIS_NAME).GetString();
            if (axisName == "x") axis = vec.x;
            if (axisName == "y") axis = vec.y;

            string dest = behaviour.GetProperty(PROP_AXIS_DEST).GetString();
            obj.SetProperty(dest, (double)axis);

            return NodeStatus.Success;
        }
    }
}
