using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace BehaviourTree
{
    public class MoveToNode : IBehaviourTreeNode
    {
        private const string PROP_TARGET_POS_SRC = "position-source";
        private const string PROP_STOP_OFFSET = "stop-offset";
        private const string PROP_MOVE_SPEED = "move-speed";
        private const string PROP_MOVE_TYPE = "move-type";
        private const string PROP_IS_MOVING_DEST = "is-moving-destination";

        public void Serialize(Behaviour behaviour)
        {
            behaviour.Properties.Add(PROP_TARGET_POS_SRC, new VariableProperty(VariableProperty.Type.String));
            behaviour.Properties.Add(PROP_STOP_OFFSET, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_SPEED, new VariableProperty(VariableProperty.Type.Number));
            behaviour.Properties.Add(PROP_MOVE_TYPE, new VariableProperty(VariableProperty.Type.Object, typeof(MoveType)));
            behaviour.Properties.Add(PROP_IS_MOVING_DEST, new VariableProperty(VariableProperty.Type.String));
        }

        public NodeStatus Tick(Tree<Behaviour>.Node self, BehaviourObject obj)
        {
            Behaviour behaviour = self.Element;
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;

            // Get the move type object + properties
            MoveType moveType = behaviour.GetProperty(PROP_MOVE_TYPE).GetObject() as MoveType;
            Vector2 currPos = obj.transform.position;

            // Check if we've already started moving, otherwise start now
            string isMovingDest = behaviour.GetProperty(PROP_IS_MOVING_DEST).GetString();
            if (!obj.HasProperty(isMovingDest)) obj.SetProperty(isMovingDest, false);
            bool isMoving = (bool)obj.GetProperty(isMovingDest);
            if (!isMoving)
            {
                // Store start position
                obj.SetProperty("startpos" + self.GetHashCode(), currPos);

                // Store target position
                float stopOffset = (float)behaviour.Properties[PROP_STOP_OFFSET].GetNumber();
                string targetPosSrc = behaviour.GetProperty(PROP_TARGET_POS_SRC).GetString();
                Vector2 targetPos = (Vector2)obj.GetProperty(targetPosSrc);
                Vector2 moveDir = (targetPos - currPos).normalized;
                obj.SetProperty("targetpos" + self.GetHashCode(), targetPos + moveDir * stopOffset);

                // Set moving true
                obj.SetProperty(isMovingDest, true);
            }

            // Update the current movement towards the destination
            Vector2 cachedTargetPos = (Vector2)obj.GetProperty("targetpos" + self.GetHashCode());
            Vector2 diff = cachedTargetPos - currPos;
            if (diff.sqrMagnitude > MathUtils.EPSILON)
            {
                // Apply the selected MoveType
                float speed = (float)behaviour.Properties[PROP_MOVE_SPEED].GetNumber();
                Vector2 startPos = (Vector2)obj.GetProperty("startpos" + self.GetHashCode());
                rigidbody2D.velocity = moveType.UpdateMove(obj, startPos, cachedTargetPos, speed);
                return NodeStatus.Running;
            }

            rigidbody2D.velocity = Vector2.zero;
            obj.SetProperty(isMovingDest, false);
            return NodeStatus.Success;
        }
    }
}
