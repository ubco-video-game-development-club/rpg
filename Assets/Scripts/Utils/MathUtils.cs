using UnityEngine;

namespace RPG
{
    public static class MathUtils
    {
        public const float EPSILON = 0.01f;

        public static int Sign(float x)
        {
            return x > EPSILON ? 1 : x < -EPSILON ? -1 : 0;
        }

        public static bool IsLayerInMask(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
