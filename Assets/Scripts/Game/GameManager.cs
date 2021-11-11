using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dialogue;

namespace RPG
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static ClassSystem ClassSystem { get; private set; }
        public static LevelingSystem LevelingSystem { get; private set; }
        public static DialogueSystem DialogueSystem { get; private set; }
        public static PopupSystem PopupSystem { get; private set; }
        public static QuestSystem QuestSystem {get; private set; }
        public static AlignmentSystem AlignmentSystem { get; private set; }
        public static MusicSystem MusicSystem { get; private set; }

        public static Player Player { get; private set; }
        public static bool IsPlayerCreated { get => Player != null; }

        [SerializeField] private Player playerPrefab;

        private UnityEvent onPlayerCreated = new UnityEvent();

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            ClassSystem = GetComponent<ClassSystem>();
            LevelingSystem = GetComponent<LevelingSystem>();
            DialogueSystem = GetComponent<DialogueSystem>();
            PopupSystem = GetComponent<PopupSystem>();
            QuestSystem = GetComponent<QuestSystem>();
            AlignmentSystem = GetComponent<AlignmentSystem>();
            MusicSystem = GetComponent<MusicSystem>();
        }

        public static void CreatePlayer() => instance.InstanceCreatePlayer();

        public static void AddPlayerCreatedListener(UnityAction listener) => instance.InstanceAddPlayerCreatedListener(listener);

        private void InstanceCreatePlayer()
        {
            Player = Instantiate(playerPrefab);
            onPlayerCreated.Invoke();
        }

        private void InstanceAddPlayerCreatedListener(UnityAction listener)
        {
            onPlayerCreated.AddListener(listener);
        }
    }
}
