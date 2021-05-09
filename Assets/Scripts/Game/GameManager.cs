using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static LevelingSystem LevelingSystem { get; private set; }
    public static DialogueSystem DialogueSystem { get; private set; }

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

        LevelingSystem = GetComponent<LevelingSystem>();
        DialogueSystem = GetComponent<DialogueSystem>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
