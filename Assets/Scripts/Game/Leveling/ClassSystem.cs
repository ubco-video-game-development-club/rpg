using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEditor;

namespace RPG
{
    public class ClassSystem : MonoBehaviour
    {
        [SerializeField] private ClassTree defaultClass;

        private ClassTree selectedClass;

        private void Awake()
        {
            SelectClass(defaultClass);
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        public void SelectClass(ClassTree classTree)
        {
            selectedClass = classTree;
        }

        public LevelingState GetLevelUpType(int level)
        {
            if (!selectedClass.ContainsTier(level)) return LevelingState.Inactive;
            switch (selectedClass.GetTierType(level))
            {
                case ClassTierType.Subclass:
                    Debug.LogError("TODO: implement subclass type!");
                    return LevelingState.Inactive;
                case ClassTierType.Skill:
                    return LevelingState.Skills;
            }
            return LevelingState.Inactive;
        }

        public LevelUpOption[] GetSkillOptions(int level)
        {
            return selectedClass.GetSkillOptions(level);
        }

        private void OnPlayerCreated()
        {
            GameManager.Player.ApplyClassBaseStats(selectedClass.GetClassBaseStats());
        }
    }
}
