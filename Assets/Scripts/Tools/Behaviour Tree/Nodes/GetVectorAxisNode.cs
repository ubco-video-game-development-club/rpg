using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public enum VectorAxis { X, Y }

    public class GetVectorAxisNode : IBehaviourTreeNode
    {
        private const string PROP_VECTOR_INPUT = "vector-input";
        private const string PROP_AXIS_NAME = "axis-name";
        private const string PROP_AXIS_OUTPUT = "axis-output";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_VECTOR_INPUT);
            behaviour.AddProperty(PROP_AXIS_NAME, new VariableProperty(VariableProperty.Type.Enum, typeof(VectorAxis)));
            behaviour.AddOutputProperty(PROP_AXIS_OUTPUT);
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;

            string vecSrc = behaviour.GetProperty(instance, PROP_VECTOR_INPUT).GetString();
            Vector2 vec = (Vector2)obj.GetProperty(vecSrc);

            float axis = 0;
            VectorAxis axisName = behaviour.GetProperty(instance, PROP_AXIS_NAME).GetEnum<VectorAxis>();
            if (axisName == VectorAxis.X) axis = vec.x;
            if (axisName == VectorAxis.Y) axis = vec.y;

            string dest = behaviour.GetProperty(instance, PROP_AXIS_OUTPUT).GetString();
            obj.SetProperty(dest, (double)axis);

            return NodeStatus.Success;
        }
    }
}
