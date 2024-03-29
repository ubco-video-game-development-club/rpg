using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

namespace RPG
{
    public class LevelManager : BehaviourObject
    {
        private static LevelManager instance;

        [SerializeField] private bool isStartLevel = false;
        public static bool IsStartLevel { get => instance.isStartLevel; }

        [SerializeField] private Transform defaultSpawnPoint;
        public static Transform DefaultSpawnPoint { get => instance.defaultSpawnPoint; }

        protected override void Awake()
        {
            base.Awake();

            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public static bool HasLevelProperty(string name)
        {
            return instance.HasProperty(name);
        }

        public static object GetLevelProperty(string name)
        {
            return instance.GetProperty(name);
        }

        public static void SetLevelProperty(string name, object property)
        {
            instance.SetProperty(name, property);
        }
    }
}
