using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public static DialogueSystem DialogueSystem { get; private set; }

    private Player player;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DialogueSystem = GetComponent<DialogueSystem>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public Player GetPlayer()
    {
        return player;
    }
}
