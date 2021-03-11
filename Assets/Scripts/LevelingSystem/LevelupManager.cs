using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelupManager : MonoBehaviour
{
    [System.Serializable]
    public struct Levelup
    {
        public Attribute attribute;
        public float amount;
    }
    public Levelup[] quackersAttributes;
    public Levelup[] flappersAttributes;
    public Levelup[] tappersAttributes;

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

    private void ApplyLevelups(Levelup[] levelups)
    {
        foreach (Levelup levelup in levelups)
        {
            int attributeLevel = player.GetAttribute(levelup.attribute);

            player.SetAttribute(levelup.attribute, attributeLevel + levelup.amount);
        }
    }
}