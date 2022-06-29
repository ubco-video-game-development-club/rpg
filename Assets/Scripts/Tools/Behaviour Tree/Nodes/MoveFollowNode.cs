using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Behaviours
{
    public class MoveFollowNode : IBehaviourTreeNode
    {
        private const string PROP_POSITION_INPUT = "position-input";
        private const string PROP_STOP_OFFSET = "stop-offset";
        private const string PROP_MOVE_SPEED = "move-speed";
        private const string PROP_MOVE_TYPE = "move-type";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.AddInputProperty(PROP_POSITION_INPUT);
            behaviour.AddProperty(PROP_STOP_OFFSET, new VariableProperty(VariableProperty.Type.Number));
            behaviour.AddProperty(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
            behaviour.AddProperty(PROP_MOVE_TYPE, new VariableProperty(VariableProperty.Type.Object, typeof(DynamicMoveType)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj, IBehaviourInstance instance)
        {
            Behaviour behaviour = self.Element;
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;

            // Get the move type object + properties
            DynamicMoveType moveType = behaviour.GetProperty(instance, PROP_MOVE_TYPE).GetObject() as DynamicMoveType;
            Vector2 currPos = obj.transform.position;

            // Calculate offset target pos
            float stopOffset = (float)behaviour.GetProperty(instance, PROP_STOP_OFFSET).GetNumber();
            string targetPosSrc = behaviour.GetProperty(instance, PROP_POSITION_INPUT).GetString();
            Vector2 targetPos = (Vector2)obj.GetProperty(targetPosSrc);
            Vector2 moveDir = (targetPos - currPos).normalized;
            Vector2 offsetPos = targetPos + moveDir * stopOffset;

            // Update the current movement towards the destination
            Vector2 diff = offsetPos - currPos;
            float speed = (float)behaviour.GetProperty(instance, PROP_MOVE_SPEED).GetNumber();
            bool moveFinished = moveType.UpdateMove(self, obj, currPos, offsetPos, speed);

            if (!moveFinished || diff.sqrMagnitude > MathUtils.STOP_EPSILON)
            {
                return NodeStatus.Running;
            }

            rigidbody2D.velocity = Vector2.zero;
            return NodeStatus.Success;
        }
    }
}
