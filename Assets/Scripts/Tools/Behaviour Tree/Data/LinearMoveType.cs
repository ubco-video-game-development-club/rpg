using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    [CreateAssetMenu(fileName = "LinearMoveType", menuName = "Move Types/Linear", order = 65)]
    public class LinearMoveType : DynamicMoveType
    {
        public override bool UpdateMove(Tree<Behaviour>.Node node, BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed)
        {
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;
            Vector2 moveDir = (endPos - rigidbody2D.position).normalized;
            rigidbody2D.velocity = moveDir * speed;
            return true;
        }
    }
}
