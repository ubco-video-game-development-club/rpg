using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class SimpleMoveNode : IBehaviourTreeNode
    {
        private const string PROP_POSITION_SRC = "position-source";
        private const string PROP_STOP_DISTANCE = "stop-distance";
        private const string PROP_MOVE_SPEED = "move-speed";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_POSITION_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_STOP_DISTANCE, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;

            string src = behaviour.Properties[PROP_POSITION_SRC].GetString();
            if (obj.HasProperty(src))
            {
                Vector2 targetPosition = (Vector2)obj.GetProperty(src);
                Vector2 currentPosition = obj.transform.position;
                Vector2 d = targetPosition - currentPosition;

                float stopDist = (float)behaviour.Properties[PROP_STOP_DISTANCE].GetNumber();
                if (d.sqrMagnitude > stopDist * stopDist)
                {
                    float speed = (float)behaviour.Properties[PROP_MOVE_SPEED].GetNumber();
                    rigidbody2D.velocity = d.normalized * speed;
                    return NodeStatus.Running;
                }

                rigidbody2D.velocity = Vector2.zero;
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}
