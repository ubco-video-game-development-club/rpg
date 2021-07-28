using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 63)]
    public class Weapon : Item
    {
        [SerializeField] private Action attack;
        public Action Attack { get => attack; }
    }
}
