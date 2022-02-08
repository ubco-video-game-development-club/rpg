using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class MoveType : ScriptableObject
    {
        public abstract void StartMove(Tree<Behaviour>.Node node, BehaviourObject obj);
        public abstract bool UpdateMove(Tree<Behaviour>.Node node, BehaviourObject obj, Vector2 startPos, Vector2 endPos, float speed);
        public abstract void EndMove(Tree<Behaviour>.Node node, BehaviourObject obj);
    }
}
