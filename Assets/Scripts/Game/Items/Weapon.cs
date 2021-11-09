using UnityEngine;
using RPG.Animation;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 63)]
    public class Weapon : Item, IInstantiable<Weapon>
    {
        [SerializeField] private Action attack;
        public Action Attack { get => attack; }

        [SerializeField] private WeaponAnimation[] animations;

        public new Weapon GetInstance()
        {
            Weapon instance = Instantiate(this);
            instance.attack = attack.GetInstance();
            return instance;
        }

        public WeaponAnimation GetAnimation(AnimationType type)
        {
            foreach (WeaponAnimation anim in animations)
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
