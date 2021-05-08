using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelingSystem : MonoBehaviour
{
    [System.Serializable]
    public struct LevelUpProperty
    {
        public PropertyName name;
        public float bonus;
    }

    [Header("Experience")]
    [SerializeField] private int xpPerLevelBase = 80;
    [SerializeField] private int xpPerLevelGrowth = 30;

    [Header("Leveling")]
    [Tooltip("Set which properties should increase when the player levels up and by how much.")]
    [SerializeField] private LevelUpProperty[] baseLevelUpBonuses;
    [Tooltip("Set which properties should increase when the player picks Quackers and by how much.")]
    [SerializeField] private LevelUpProperty[] quackersLevelUpBonuses;
    [Tooltip("Set which properties should increase when the player picks Flappers and by how much.")]
    [SerializeField] private LevelUpProperty[] flappersLevelUpBonuses;
    [Tooltip("Set which properties should increase when the player picks Tappers and by how much.")]
    [SerializeField] private LevelUpProperty[] tappersLevelUpBonuses;

    public int XP
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.XP); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.XP, value); }
    }

    public int Level
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.Level); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.Level, value); }
    }

    public int LevelUps
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.LevelUps); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.LevelUps, value); }
    }

    public int Quackers
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.Quackers); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.Quackers, value); }
    }

    public int Flappers
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.Flappers); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.Flappers, value); }
    }

    public int Tappers
    {
        get { return GameManager.Player.GetProperty<int>(PropertyName.Tappers); }
        private set { GameManager.Player.SetProperty<int>(PropertyName.Tappers, value); }
    }

    void Awake()
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
        GUILayout.BeginArea(new Rect(10, 10, 150, Screen.height / 2 - 15));

        GUILayout.Label($"XP: {XP}");
        GUILayout.Label($"Level: {Level}");
        GUILayout.Label($"Available Level Ups: {LevelUps}");

        if (LevelUps > 0)
        {
            if (GUILayout.Button("Level Up Quackers!"))
            {
                LevelUpQuackers();
            }
            if (GUILayout.Button("Level Up Flappers!"))
            {
                LevelUpFlappers();
            }
            if (GUILayout.Button("Level Up Tappers!"))
            {
                LevelUpTappers();
            }
        }

        GUILayout.EndArea();
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

    private void ApplyLevelUp(LevelUpProperty[] pickedLevelUpProperties)
    {
        LevelUps--;
        Level++;

        Player player = GameManager.Player;

        // Apply the base level up bonuses
        foreach (LevelUpProperty prop in baseLevelUpBonuses)
        {
            float currentValue = player.GetProperty<float>(prop.name);
            player.SetProperty<float>(prop.name, currentValue + prop.bonus);
        }

        // Apply the level up bonuses for the picked type
        foreach (LevelUpProperty prop in pickedLevelUpProperties)
        {
            float currentValue = player.GetProperty<float>(prop.name);
            player.SetProperty<float>(prop.name, currentValue + prop.bonus);
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
