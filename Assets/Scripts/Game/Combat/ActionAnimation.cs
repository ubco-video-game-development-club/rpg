using UnityEngine;
using RPG.Animation;

namespace RPG
{
    [System.Serializable]
    public class ActionAnimation
    {
        [SerializeField] private AnimationType type;
        public AnimationType Type { get => type; }

        [SerializeField] private AnimationSet8D avatarAnimation;
        public AnimationSet8D AvatarAnimation { get => avatarAnimation; }

        [SerializeField] private AnimationSet8D weaponAnimation;
        public AnimationSet8D WeaponAnimation { get => weaponAnimation; }
    }
}
