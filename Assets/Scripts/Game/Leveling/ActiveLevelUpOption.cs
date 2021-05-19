using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New ActiveAbility", menuName = "LevelUp Options/Active Ability", order = 60)]
    public class ActiveLevelUpOption : LevelUpOption
    {
        [SerializeField] private Action ability;

        public override void Apply(Player player)
        {
            player.AvailableAbilities.Add(ability);
        }

        public override void UnApply(Player player)
        {
            player.AvailableAbilities.Remove(ability);
        }
    }
}
