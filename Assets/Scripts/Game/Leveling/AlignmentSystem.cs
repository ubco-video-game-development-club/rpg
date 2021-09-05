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
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) && OnAlignmentChanged != null)
            {
                TestAddPoints();
            }
        }

        private void OnPlayerCreated()
        {
            Morals = 0;
            Leanings = 0;
            Sexiness = 0;
        }

        public void UpdateMorals(float points)
        {
            Debug.Log("Update Morals " + points);
            Morals += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }

        public void UpdateLeanings(float points)
        {
            Debug.Log("Update Leanings " + points);
            Leanings += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }

        public void UpdateSexiness(float points)
        {
            Debug.Log("Update Sexiness " + points);
            Sexiness += points;
            OnAlignmentChanged.Invoke(Morals, Leanings, Sexiness);
        }

        public void TestAddPoints()
        {
            UpdateMorals(.1f);
            UpdateLeanings(.1f);
        }
    }
}
