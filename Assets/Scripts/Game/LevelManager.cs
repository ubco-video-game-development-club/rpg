using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

namespace RPG
{
    public class LevelManager : BehaviourObject
    {
        private static LevelManager instance;

        protected override void Awake()
        {
            base.Awake();

            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
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
