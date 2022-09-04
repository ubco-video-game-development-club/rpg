using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Animation;

namespace RPG
{
    public abstract class Action : ScriptableObject, IInstantiable<Action>
    {
        [SerializeField] private Effect[] onHitEffects;
        public Effect[] OnHitEffects { get => onHitEffects; }

        [SerializeField] private Effect[] onKillEffects;
        public Effect[] OnKillEffects { get => onKillEffects; }

        [SerializeField] private bool useWeaponAnimation = false;
        public bool UseWeaponAnimation { get => useWeaponAnimation; }

        [SerializeField] private AnimationType animationType;
        public AnimationType AnimationType { get => animationType; }

        [SerializeField] private Animation2D animation;
        public Animation2D Animation { get => animation; }

        [SerializeField] private float cooldown = 1f;
        public float Cooldown { get => cooldown; }

        public bool Enabled { get; set; }

        private UnityEvent<Actor> onHit = new UnityEvent<Actor>();
        public UnityEvent<Actor> OnHit { get => onHit; }

        private UnityEvent<Actor> onKill = new UnityEvent<Actor>();
        public UnityEvent<Actor> OnKill { get => onKill; }

        public abstract void Invoke(ActionData data);

        public Action GetInstance() => Instantiate(this);
    }
}
