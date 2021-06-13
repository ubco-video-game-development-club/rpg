using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static ClassSystem ClassSystem { get; private set; }
        public static LevelingSystem LevelingSystem { get; private set; }
        public static DialogueSystem DialogueSystem { get; private set; }

        public static Player Player { get; private set; }

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

            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
}
