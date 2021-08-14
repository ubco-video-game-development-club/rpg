using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField] private Effect[] onHitEffects;
        public Effect[] OnHitEffects { get => onHitEffects; }

        [SerializeField] private Effect[] onKillEffects;
        public Effect[] OnKillEffects { get => onKillEffects; }

        [SerializeField] private RuntimeAnimatorController animationController;
        public RuntimeAnimatorController AnimationController { get => animationController; }

        [SerializeField] private float cooldown = 1f;
        public float Cooldown { get => cooldown; }

        public bool Enabled { get; set; }

        private UnityEvent<Actor> onHit = new UnityEvent<Actor>();
        public UnityEvent<Actor> OnHit { get => onHit; }

        private UnityEvent<Actor> onKill = new UnityEvent<Actor>();
        public UnityEvent<Actor> OnKill { get => onKill; }

        public abstract void Invoke(ActionData data);
    }
}
