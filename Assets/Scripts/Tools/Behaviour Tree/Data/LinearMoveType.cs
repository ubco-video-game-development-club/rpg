using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "LinearMoveType", menuName = "Move Types/Linear", order = 65)]
    public class LinearMoveType : MoveType
    {
        public override Vector2 UpdateMove(BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed)
        {
            Vector2 moveDir = (endPos - startPos).normalized;
            return moveDir * speed;
        }
    }
}
