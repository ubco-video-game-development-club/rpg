using UnityEngine;

namespace RPG
{
    public struct ActionData
    {
        public LayerMask targetMask;
        public Vector2 origin;
        public Vector2 target;

        public ActionData(LayerMask targetMask)
        {
            this.targetMask = targetMask;
            this.origin = Vector2.zero;
            this.target = Vector2.zero;
        }
    }
}
