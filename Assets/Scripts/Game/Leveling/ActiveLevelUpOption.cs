using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class ActiveLevelUpOption : LevelUpOption
    {
        [SerializeField] private ActiveAbility ability;

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
