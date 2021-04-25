using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private static LevelingSystem levelingSystem;
    public static LevelingSystem LevelingSystem { get { return levelingSystem; } }

    private static Player player;
    public static Player Player { get { return player; } }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        levelingSystem = GetComponent<LevelingSystem>();
    }
}
