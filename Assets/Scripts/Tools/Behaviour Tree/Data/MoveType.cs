using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class MoveType : ScriptableObject
    {
        public abstract Vector2 UpdateMove(BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed);
    }
}
