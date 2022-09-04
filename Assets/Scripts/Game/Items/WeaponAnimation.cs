using UnityEngine;
using RPG.Animation;

namespace RPG
{
    [System.Serializable]
    public class WeaponAnimation
    {
        [SerializeField] private AnimationType type;
        public AnimationType Type { get => type; }

        [SerializeField] private Animation2D avatarAnimation;
        public Animation2D AvatarAnimation { get => avatarAnimation; }

        [SerializeField] private Animation2D animation;
        public Animation2D Animation { get => animation; }
    }
}
