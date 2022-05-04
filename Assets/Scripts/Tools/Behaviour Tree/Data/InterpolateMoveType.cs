using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [CreateAssetMenu(fileName = "InterpolateMoveType", menuName = "Move Types/Interpolate", order = 65)]
    public class InterpolateMoveType : MoveType
    {
        private enum MoveInterpolateType
        {
            EaseOut,
            EaseInOut,
            Bounce
        }

        [SerializeField] private MoveInterpolateType interpolateType;

        public override void StartMove(Tree<Behaviour>.Node node, BehaviourObject obj)
        {
            obj.SetProperty("movetime" + node.GetHashCode(), 0f);
        }

        public override bool UpdateMove(Tree<Behaviour>.Node node, BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed)
        {
            // Calculate path
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;
            Vector2 path = endPos - startPos;

            // Calculate interpolation
            float moveTime = (float)obj.GetProperty("movetime" + node.GetHashCode());
            float lerpTime = path.magnitude / speed;
            Vector2 nextPos = startPos + Interpolate(path, moveTime / lerpTime);

            // Update position and interpolant
            rigidbody2D.MovePosition(nextPos);
            float newMoveTime = moveTime + Time.deltaTime;
            obj.SetProperty("movetime" + node.GetHashCode(), newMoveTime);
            return newMoveTime / lerpTime >= 1f;
        }

        public override void EndMove(Tree<Behaviour>.Node node, BehaviourObject obj)
        {
            obj.RemoveProperty("movetime" + node.GetHashCode());
        }

        private Vector2 Interpolate(Vector2 path, float progress)
        {
            switch (interpolateType)
            {
                case MoveInterpolateType.EaseOut:
                    return EaseOut(path, progress);
                case MoveInterpolateType.EaseInOut:
                    return EaseInOut(path, progress);
                case MoveInterpolateType.Bounce:
                    return Bounce(path, progress);
            }
            return Vector2.zero;
        }

        private Vector2 EaseOut(Vector2 path, float progress)
        {
            return path * Mathf.Sin((progress * Mathf.PI) / 2);
        }

        private Vector2 EaseInOut(Vector2 path, float progress)
        {
            return path * -(Mathf.Cos(Mathf.PI * progress) - 1) / 2;
        }

        private Vector2 Bounce(Vector2 path, float progress)
        {
            float c4 = (2f * Mathf.PI) / 3f;
            if (progress == 0)
                return Vector2.zero;
            else if (progress == 1)
                return path;
            else
                return path * (Mathf.Pow(2f, -10f * progress) * Mathf.Sin((progress * 10f - 0.75f) * c4) + 1);
        }
    }
}
