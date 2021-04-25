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
        public float value;
    }

    [Header("Experience")]
    [SerializeField] private int xpPerLevelBase = 80;
    [SerializeField] private int xpPerLevelGrowth = 30;

    [Header("Leveling")]
    [SerializeField] private LevelUpProperty[] quackersProperties;
    [SerializeField] private LevelUpProperty[] flappersProperties;
    [SerializeField] private LevelUpProperty[] tappersProperties;

    private UnityEvent<int> onXPChanged = new UnityEvent<int>();
    public UnityEvent<int> OnXPChanged { get { return onXPChanged; } }

    private UnityEvent<int> onLevelChanged = new UnityEvent<int>();
    public UnityEvent<int> OnLevelChanged { get { return onLevelChanged; } }

    private UnityEvent<int> onLevelUpsChanged = new UnityEvent<int>();
    public UnityEvent<int> OnLevelUpsChanged { get { return onLevelUpsChanged; } }

    private int xp;
    private int level;
    private int levelUps;

    void Awake()
    {
        xp = 0;
        level = ToLevel(xp);

        Debug.Log(ToXP(1));
        Debug.Log(ToXP(2));
        Debug.Log(ToXP(3));
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 20), $"XP: {xp}");
        GUI.Label(new Rect(10, 30, 150, 20), $"Level: {level}");
        GUI.Label(new Rect(10, 50, 150, 20), $"Available Level Ups: {levelUps}");

        if (levelUps > 0)
        {
            if (GUI.Button(new Rect(10, 70, 150, 20), "Level Up Quackers!"))
            {
                LevelUpQuackers();
            }
            if (GUI.Button(new Rect(10, 90, 150, 20), "Level Up Flappers!"))
            {
                LevelUpFlappers();
            }
            if (GUI.Button(new Rect(10, 110, 150, 20), "Level Up Tappers!"))
            {
                LevelUpTappers();
            }
        }
    }

    public void AddXP(int amount)
    {
        xp += amount;
        onXPChanged.Invoke(xp);

        int newLevelUps = ToLevel(this.xp) - level;
        if (newLevelUps > levelUps) onLevelUpsChanged.Invoke(newLevelUps);
        levelUps = newLevelUps;
    }

    public void LevelUpQuackers()
    {
        ApplyLevelUp(quackersProperties);
    }

    public void LevelUpFlappers()
    {
        ApplyLevelUp(flappersProperties);
    }

    public void LevelUpTappers()
    {
        ApplyLevelUp(tappersProperties);
    }

    private void ApplyLevelUp(LevelUpProperty[] levelUpProperties)
    {
        levelUps--;
        onLevelUpsChanged.Invoke(levelUps);

        level++;
        onLevelChanged.Invoke(level);

        Player player = GameManager.Player;
        foreach (LevelUpProperty prop in levelUpProperties)
        {
            float currentValue = player.GetProperty<float>(prop.name);
            player.SetProperty<float>(prop.name, currentValue + prop.value);
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
