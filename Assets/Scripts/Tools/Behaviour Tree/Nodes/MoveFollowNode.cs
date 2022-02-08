using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class MoveFollowNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_POS_SRC = "position-source";
        private const string PROP_STOP_OFFSET = "stop-offset";
        private const string PROP_MOVE_SPEED = "move-speed";
        private const string PROP_MOVE_TYPE = "move-type";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_POS_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_STOP_OFFSET, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_TYPE, new VariableProperty(VariableProperty.Type.Object, typeof(MoveType)));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;

            // Get the move type object + properties
            MoveType moveType = behaviour.GetProperty(PROP_MOVE_TYPE).GetObject() as MoveType;
            Vector2 currPos = obj.transform.position;

            // Calculate offset target pos
            float stopOffset = (float)behaviour.Properties[PROP_STOP_OFFSET].GetNumber();
            string targetPosSrc = behaviour.GetProperty(PROP_TARGET_POS_SRC).GetString();
            Vector2 targetPos = (Vector2)obj.GetProperty(targetPosSrc);
            Vector2 moveDir = (targetPos - currPos).normalized;
            Vector2 offsetPos = targetPos + moveDir * stopOffset;

            // Update the current movement towards the destination
            Vector2 diff = offsetPos - currPos;
            float speed = (float)behaviour.Properties[PROP_MOVE_SPEED].GetNumber();
            bool moveFinished = moveType.UpdateMove(self, obj, currPos, offsetPos, speed);

            if (!moveFinished || diff.sqrMagnitude > MathUtils.EPSILON)
            {
                return NodeStatus.Running;
            }

            rigidbody2D.velocity = Vector2.zero;
            return NodeStatus.Success;
        }
    }
}
