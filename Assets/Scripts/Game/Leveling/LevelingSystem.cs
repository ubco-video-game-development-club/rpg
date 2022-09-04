using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public enum LevelingState
    {
        Inactive, QFT, Skills
    }

    public class LevelingSystem : MonoBehaviour
    {
        [Header("Experience")]
        [SerializeField] private int xpPerLevelBase = 80;
        [SerializeField] private int xpPerLevelGrowth = 30;

        [Header("Leveling")]
        [Tooltip("Set which properties should increase when the player levels up and by how much.")]
        [SerializeField] private EntityProperty[] baseLevelUpBonuses;
        [Tooltip("Set which properties should increase when the player picks Quackers and by how much.")]
        [SerializeField] private EntityProperty[] quackersLevelUpBonuses;
        [Tooltip("Set which properties should increase when the player picks Flappers and by how much.")]
        [SerializeField] private EntityProperty[] flappersLevelUpBonuses;
        [Tooltip("Set which properties should increase when the player picks Tappers and by how much.")]
        [SerializeField] private EntityProperty[] tappersLevelUpBonuses;

        public int XP
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.XP);
            private set => GameManager.Player.SetProperty<int>(PropertyName.XP, value);
        }

        public int Level
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Level);
            private set => GameManager.Player.SetProperty<int>(PropertyName.Level, value);
        }

        public int LevelUps
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.LevelUps);
            private set => GameManager.Player.SetProperty<int>(PropertyName.LevelUps, value);
        }

        public int Quackers
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Quackers);
            private set => GameManager.Player.SetProperty<int>(PropertyName.Quackers, value);
        }

        public int Flappers
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Flappers);
            private set => GameManager.Player.SetProperty<int>(PropertyName.Flappers, value);
        }

        public int Tappers
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Tappers);
            private set => GameManager.Player.SetProperty<int>(PropertyName.Tappers, value);
        }

        private LevelingState levelingState = LevelingState.Inactive;
        private LevelUpOption[] skillOptions;

        void Awake()
        {
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            XP = 0;
            Level = ToLevel(XP);
            LevelUps = 0;
            Quackers = 0;
            Flappers = 0;
            Tappers = 0;
        }

        void OnGUI()
        {
            if (!GameManager.IsPlayerCreated) return;

            // GUILayout.BeginArea(new Rect(10, 10, 150, Screen.height / 2 - 15));
            // GUILayout.Label($"XP: {XP}");
            // GUILayout.Label($"Level: {Level}");
            // GUILayout.Label($"Available Level Ups: {LevelUps}");
            // if (LevelUps > 0)
            // {
            //     if (levelingState != LevelingState.Inactive) GUI.enabled = false;
            //     if (GUILayout.Button("Level Up")) levelingState = LevelingState.QFT;
            //     GUI.enabled = true;
            // }
            // GUILayout.EndArea();

            // if (levelingState == LevelingState.QFT)
            // {
            //     GUILayout.BeginArea(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 150, 150, 300));
            //     GUILayout.Label("Select a levelup option!");
            //     GUILayout.Space(10);
            //     if (GUILayout.Button("Level Up Quackers!")) LevelUpQuackers();
            //     GUILayout.Space(10);
            //     if (GUILayout.Button("Level Up Flappers!")) LevelUpFlappers();
            //     GUILayout.Space(10);
            //     if (GUILayout.Button("Level Up Tappers!")) LevelUpTappers();
            //     GUILayout.EndArea();
            // }

            // if (levelingState == LevelingState.Skills)
            // {
            //     GUILayout.BeginArea(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 150, 150, 300));
            //     GUILayout.Label("Select one of the following skills!");
            //     GUILayout.Space(10);
            //     foreach (LevelUpOption option in skillOptions)
            //     {
            //         if (GUILayout.Button(option.Title)) SelectSkill(option);
            //         GUILayout.Label(option.Description);
            //         GUILayout.Space(10);
            //     }
            //     GUILayout.EndArea();
            // }
        }

        public void AddXP(int amount)
        {
            XP += amount;

            int newLevelUps = ToLevel(XP) - Level;
            if (newLevelUps > LevelUps) LevelUps = newLevelUps;
        }

        public void LevelUpQuackers()
        {
            ApplyLevelUp(quackersLevelUpBonuses);
            Quackers++;
        }

        public void LevelUpFlappers()
        {
            ApplyLevelUp(flappersLevelUpBonuses);
            Flappers++;
        }

        public void LevelUpTappers()
        {
            ApplyLevelUp(tappersLevelUpBonuses);
            Tappers++;
        }

        public void SelectSkill(LevelUpOption skill)
        {
            skill.Select();
            levelingState = LevelingState.Inactive;
        }

        public int GetPotentialLevel()
        {
            return Level + LevelUps;
        }

        public int GetRequiredXP()
        {
            return ToXP(Level + 1);
        }

        private void ApplyLevelUp(EntityProperty[] selectedLevelUpBonuses)
        {
            LevelUps--;
            Level++;

            // Apply the base level up bonuses
            foreach (EntityProperty bonus in baseLevelUpBonuses)
            {
                ApplyBonus(bonus);
            }

            // Apply the level up bonuses for the selected path
            foreach (EntityProperty bonus in selectedLevelUpBonuses)
            {
                ApplyBonus(bonus);
            }

            // Update leveling state
            levelingState = GameManager.ClassSystem.GetLevelUpType(Level);
            if (levelingState == LevelingState.Skills)
            {
                skillOptions = GameManager.ClassSystem.GetSkillOptions(Level);
                if (skillOptions.Length == 0) levelingState = LevelingState.Inactive;
            }
        }

        private void ApplyBonus(EntityProperty bonus)
        {
            // Apply the level up bonus to the player based on the property type
            Player player = GameManager.Player;
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

        ///<summary>Convert a total XP amount to the level it represents.</summary>
        private int ToLevel(int xp)
        {
            int a = xpPerLevelBase;
            int b = xpPerLevelGrowth;
            return Mathf.FloorToInt(((-a + 0.5f * b + Mathf.Sqrt(a * a - a * b + 0.25f * b * b + 2f * b * xp)) / b) + 1);
        }

        ///<summary>Convert a level to the amount of total XP it represents.</summary>
        private int ToXP(int level)
        {
            int a = xpPerLevelBase;
            int b = xpPerLevelGrowth;
            int c = level - 1;
            return Mathf.FloorToInt(0.5f * b * c * c + (a - 0.5f * b) * c);
        }
    }
}
