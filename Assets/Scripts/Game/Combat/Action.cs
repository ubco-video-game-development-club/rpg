using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Action", menuName = "Action", order = 59)]
    public class Action : ScriptableObject
    {
        [SerializeField] private Effect[] effects;

        [SerializeField] private RuntimeAnimatorController animationController;
        public RuntimeAnimatorController AnimationController { get => animationController; }

        [SerializeField] private float cooldown = 1f;
        public float Cooldown { get => cooldown; }

        public bool Enabled { get; set; }

        public void Invoke(ActionData data)
        {
            foreach (Effect effect in effects)
            {
                effect.Invoke(data);
            }
        }
    }
}
