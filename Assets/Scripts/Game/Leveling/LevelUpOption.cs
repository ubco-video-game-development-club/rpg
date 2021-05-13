using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class LevelUpOption : ScriptableObject
    {
        [SerializeField] private string optionName;
        [SerializeField] [TextArea] private string optionDescription;

        public abstract void Apply(Player player);
        public abstract void UnApply(Player player);
    }
}
