using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New LevelUpOption", menuName = "LevelUp Option", order = 60)]
    public class LevelUpOption : Upgrade
    {
        [SerializeField] private Action[] abilities;
        [SerializeField] private string title;
        [SerializeField] [TextArea] private string description;

        public Action[] Abilities { get => abilities; }
        public string Title { get => title; }
        public string Description { get => description; }

        public override void Apply(Entity entity)
        {
            base.Apply(entity);

            if (entity.TryGetComponent<Player>(out Player player))
            {
                foreach (Action ability in abilities)
                {
                    player.AvailableAbilities.Add(ability);
                }
            }
        }
    }
}
