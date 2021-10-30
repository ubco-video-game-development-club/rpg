using UnityEngine;
using RPG.Animation;

namespace RPG
{
    [System.Serializable]
    public class WeaponAnimation
    {
        [SerializeField] private AnimationType type;
        public AnimationType Type { get => type; }

        [SerializeField] private AnimationSet8D avatarAnimation;
        public AnimationSet8D AvatarAnimation { get => avatarAnimation; }

        [SerializeField] private AnimationSet8D animation;
        public AnimationSet8D Animation { get => animation; }
    }
}
