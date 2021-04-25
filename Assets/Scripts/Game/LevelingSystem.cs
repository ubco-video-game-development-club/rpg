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

    private UnityEvent<int> onLevelUpsChanged = new UnityEvent<int>();
    public UnityEvent<int> OnLevelUpsChanged { get { return onLevelUpsChanged; } }

    private int xp;
    private int level;

    public void AddXP(int amount)
    {
        xp += amount;
        onXPChanged.Invoke(xp);

        int levelups = ToLevel(this.xp) - level;
        if (levelups > 0) onLevelUpsChanged.Invoke(levelups);
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
        return Mathf.FloorToInt((-a + 0.5f * b + Mathf.Sqrt(a * a - a * b + 0.25f * b * b + 2f * b * xp)) / b);
    }

    ///<summary>Convert a level to the amount of total XP it represents.</summary>
    private int ToXP(int level)
    {
        int a = xpPerLevelBase;
        int b = xpPerLevelGrowth;
        return Mathf.FloorToInt(0.5f * b * level * level + (a - 0.5f * b) * level);
    }
}
