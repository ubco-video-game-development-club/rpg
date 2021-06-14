using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassEditor;

namespace RPG
{
    public class ClassSystem : MonoBehaviour
    {
        [SerializeField] private ClassTree selectedClass;

        private LinkedList<ClassNode> levelUpPath = new LinkedList<ClassNode>();

        public void SelectClass(ClassTree classTree)
        {
            GameManager.CreatePlayer();

            // TODO: save the selected class to the player?
            selectedClass = classTree;

            ClassData data = classTree.GetClassData();
            GameManager.Player.SetProperty<int>(PropertyName.MaxHealth, data.health);
            GameManager.Player.SetProperty<int>(PropertyName.Health, data.health);

            // levelUpPath.AddFirst(new LinkedListNode<ClassNode>(selectedClass.G));
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
    }
}
