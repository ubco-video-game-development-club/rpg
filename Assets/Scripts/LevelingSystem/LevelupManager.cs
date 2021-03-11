using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelupManager : MonoBehaviour
{
    [System.Serializable]
    public struct LevelupAttribute
    {
        public Attribute attribute;
        public float levelupAmount;
    }
    public LevelupAttribute[] quackersAttributes;
    public LevelupAttribute[] flappersAttributes;
    public LevelupAttribute[] tappersAttributes;
    
    private Player player;

    private void Start() {
        player = GameManager.Instance.GetPlayer();    
    }

    
    //TODO Class levelups
    //TODO Key level levelups
    //TODO Get reference to player in order to increase attribute on player
    public void OnLevelUp(){
        
    }
    
    public void QuackerLevelUp(){
        foreach (LevelupAttribute attribute in quackersAttributes)
        {
            attribute+=levelupAmount;
        }
    }

    public void FlapperLevelUp(){
        foreach (LevelUpAttribute attribute in quackersAttributes)
        {
            
        }
    }

    public void TapperLevelUp(){
        foreach (LevelUpAttribute attribute in quackersAttributes)
        {
            
        }
    }
}
