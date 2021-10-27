using UnityEngine;
using RPG.Animation;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 63)]
    public class Weapon : Item
    {
        [SerializeField] private Action attack;
        public Action Attack { get => attack; }

        [SerializeField] private ActionAnimation[] animations;
        
        public ActionAnimation GetAnimation(AnimationType type)
        {
            foreach (ActionAnimation anim in animations)
            {
                if (anim.Type == type)
                {
                    return anim;
                }
            }
            return null;
        }
    }
}
