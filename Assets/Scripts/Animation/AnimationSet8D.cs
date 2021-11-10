using UnityEngine;

namespace RPG.Animation
{
    [CreateAssetMenu(fileName = "New AnimationSet8D", menuName = "AnimationSet8D", order = 400)]
    public class AnimationSet8D : ScriptableObject
    {
        [SerializeField] private AnimationClip bottom;
        [SerializeField] private AnimationClip bottomLeft;
        [SerializeField] private AnimationClip bottomRight;
        [SerializeField] private AnimationClip left;
        [SerializeField] private AnimationClip right;
        [SerializeField] private AnimationClip top;
        [SerializeField] private AnimationClip topLeft;
        [SerializeField] private AnimationClip topRight;

        public AnimationClip Get(Vector2Int dir)
        {
            if (dir.x == 0 && dir.y < 0) return bottom;
            if (dir.x < 0 && dir.y < 0) return bottomLeft;
            if (dir.x > 0 && dir.y < 0) return bottomRight;
            if (dir.x < 0 && dir.y == 0) return left;
            if (dir.x > 0 && dir.y == 0) return right;
            if (dir.x == 0 && dir.y > 0) return top;
            if (dir.x < 0 && dir.y > 0) return topLeft;
            if (dir.x > 0 && dir.y > 0) return topRight;
            else return null;
        }
    }
}
