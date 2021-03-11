using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class XPManager
{
    //as xpMultiplier goes up, the amount of experience required to levelup will go up
    [SerializeField] private const double XP_TO_LEVEL_FACTOR = 10;
    [SerializeField] private int levelupBase = 75;

    private int experience;
    private int currentLevel;

    //
    public class ExperienceChangedEvent : UnityEvent<int> { };
    private ExperienceChangedEvent onExperienceChanged = new ExperienceChangedEvent();


    // 
    public class AvailableLevelUpsChangedEvent : UnityEvent<int> { };
    private AvailableLevelUpsChangedEvent onAvailableLevelUpsChanged = new AvailableLevelUpsChangedEvent();

    public void AddXP(int XP)
    {
        experience += XP;
        onExperienceChanged.Invoke(experience);
    }
    //Check to see if we have a potential levelup
    //If we do, call event with number of available levelups
    public void HandleExperienceChanged(int xp)
    {
        int potentialLevel = CalculatePotentialLevel(xp);
        if (potentialLevel > currentLevel)
        {
            onAvailableLevelUpsChanged.Invoke(potentialLevel - currentLevel);
        }
    }

    public int CalculatePotentialLevel(int totalXP)
    {
        //https://en.uesp.net/wiki/Skyrim:Leveling
        return (int)Math.Floor((-25 + Math.Sqrt(8 * totalXP + 1225)) / XP_TO_LEVEL_FACTOR);
    }
}
