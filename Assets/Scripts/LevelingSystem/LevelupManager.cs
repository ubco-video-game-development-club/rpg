using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelupManager : MonoBehaviour
{
    [System.Serializable]
    public struct LevelupAttribute
    {
        public PropertyName propertyName;
        public float value;
    }

    private LevelupAttribute[] quackersAttributes;
    public LevelupAttribute[] flappersAttributes;
    public LevelupAttribute[] tappersAttributes;

    private Player player;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }

    //TODO Class levelups
    //TODO Key level levelups
    //TODO Get reference to player in order to increase attribute on player
    public void OnLevelup()
    {

    }

    public void QuackerLevelup()
    {
        ApplyLevelups(quackersAttributes);
    }

    public void FlapperLevelup()
    {
        ApplyLevelups(flappersAttributes);
    }

    public void TapperLevelup()
    {
        ApplyLevelups(tappersAttributes);
    }

    private void ApplyLevelups(LevelupAttribute[] levelupAttributes)
    {
        foreach (LevelupAttribute attr in levelupAttributes)
        {
            float currentValue = player.GetProperty<float>(attr.propertyName);
            player.SetProperty<float>(attr.propertyName, currentValue + attr.value);
        }
    }
}