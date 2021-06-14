using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class LevelUpOption : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] [TextArea] private string description;

        public string Title { get => title; }
        public string Description { get => description; }

        public abstract void Apply(Player player);
        public abstract void UnApply(Player player);
    }
}
