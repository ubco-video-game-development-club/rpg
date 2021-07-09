using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New SimplePassive", menuName = "LevelUp Options/Simple Passive", order = 60)]
    public class SimplePassiveLevelUpOption : LevelUpOption
    {
        [SerializeField] private EntityProperty[] levelUpBonuses;

        public override void Apply(Player player)
        {
            foreach (EntityProperty bonus in levelUpBonuses)
            {
                switch (bonus.Type)
                {
                    case PropertyType.Int:
                        int currentIntValue = player.GetProperty<int>(bonus.Name);
                        player.SetProperty<int>(bonus.Name, currentIntValue + bonus.Value);
                        break;
                    case PropertyType.Float:
                        float currentFloatValue = player.GetProperty<float>(bonus.Name);
                        player.SetProperty<float>(bonus.Name, currentFloatValue + bonus.Value);
                        break;
                    default:
                        Debug.LogError("Level Up bonus type must be either Int or Float!");
                        break;
                }
            }
        }

        public override void UnApply(Player player)
        {
            foreach (EntityProperty bonus in levelUpBonuses)
            {
                switch (bonus.Type)
                {
                    case PropertyType.Int:
                        int currentIntValue = player.GetProperty<int>(bonus.Name);
                        player.SetProperty<int>(bonus.Name, currentIntValue - bonus.Value);
                        break;
                    case PropertyType.Float:
                        float currentFloatValue = player.GetProperty<float>(bonus.Name);
                        player.SetProperty<float>(bonus.Name, currentFloatValue - bonus.Value);
                        break;
                    default:
                        Debug.LogError("Level Up bonus type must be either Int or Float!");
                        break;
                }
            }
        }
    }
}
