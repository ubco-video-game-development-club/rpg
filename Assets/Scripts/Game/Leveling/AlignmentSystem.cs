using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public class AlignmentSystem : MonoBehaviour
    {
        public UnityEvent<float, float, float> OnAlignmentChanged { get; private set; }

        public float Morals
        {
            get => GameManager.Player.GetProperty<float>(PropertyName.Morals);
            private set => GameManager.Player.SetProperty<float>(PropertyName.Morals, value);
        }

        public float Leanings
        {
            get => GameManager.Player.GetProperty<float>(PropertyName.Leanings);
            private set => GameManager.Player.SetProperty<float>(PropertyName.Leanings, value);
        }

        public float Sexiness
        {
            get => GameManager.Player.GetProperty<float>(PropertyName.Sexiness);
            private set => GameManager.Player.SetProperty<float>(PropertyName.Sexiness, value);
        }

        private void Awake()
        {
            OnAlignmentChanged = new UnityEvent<float, float, float>();
        }

        private void Start()
        {
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            Morals = 0;
            Leanings = 0;
            Sexiness = 0;
        }

        public void UpdateMorals(float points)
        {
            if (!GameManager.IsPlayerCreated)
            {
                return;
            }

            Morals += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }

        public void UpdateLeanings(float points)
        {
            if (!GameManager.IsPlayerCreated)
            {
                return;
            }

            Leanings += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }

        public void UpdateSexiness(float points)
        {
            if (!GameManager.IsPlayerCreated)
            {
                return;
            }

            Sexiness += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }
    }
}
