namespace RPG
{
    public static class MathUtils
    {
        public const float EPSILON = 0.01f;

        public static int Sign(float x)
        {
            return x > EPSILON ? 1 : x < -EPSILON ? -1 : 0;
        }
    }
}
