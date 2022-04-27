using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public abstract class DynamicMoveType : MoveType
    {
        public override void StartMove(Tree<Behaviour>.Node node, BehaviourObject obj) { }
        public override void EndMove(Tree<Behaviour>.Node node, BehaviourObject obj) { }
    }
}
