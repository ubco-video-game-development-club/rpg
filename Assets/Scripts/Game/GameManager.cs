using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static LevelingSystem LevelingSystem { get; private set; }
        public static DialogueSystem DialogueSystem { get; private set; }
        public static PopupSystem PopupSystem { get; private set; }
        public static QuestSystem QuestSystem {get; private set; }

        public static Player Player { get; private set; }

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
            PopupSystem = GetComponent<PopupSystem>();
            QuestSystem = GetComponent<QuestSystem>();

            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
}
