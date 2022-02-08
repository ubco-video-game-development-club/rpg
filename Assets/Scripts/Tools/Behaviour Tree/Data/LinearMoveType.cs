using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(fileName = "LinearMoveType", menuName = "Move Types/Linear", order = 65)]
    public class LinearMoveType : MoveType
    {
        public override void StartMove(Tree<Behaviour>.Node node, BehaviourObject obj) { }

        public override bool UpdateMove(Tree<Behaviour>.Node node, BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed)
        {
            Rigidbody2D rigidbody2D = ((Agent)obj).Rigidbody2D;
            Vector2 moveDir = (endPos - startPos).normalized;
            rigidbody2D.velocity = moveDir * speed;
            return true;
        }

        public override void EndMove(Tree<Behaviour>.Node node, BehaviourObject obj) { }
    }
}
