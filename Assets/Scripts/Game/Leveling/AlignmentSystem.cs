using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace RPG
{
    public class AlignmentSystem : MonoBehaviour
    {
        public UnityEvent<int> onAligneChanged;

        public int Sexiness
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Sexiness);
            set => GameManager.Player.SetProperty<int>(PropertyName.Sexiness, value);
        }

        public int Morals
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Morals);
            set => GameManager.Player.SetProperty<int>(PropertyName.Morals, value);
        }

        public int Leanings
        {
            get => GameManager.Player.GetProperty<int>(PropertyName.Leanings);
            set => GameManager.Player.SetProperty<int>(PropertyName.Leanings, value);
        }

        void Awake()
        {
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
            
            onAligneChanged.AddListener(UpdateMorals);
            onAligneChanged.AddListener(UpdateLeanings);
            onAligneChanged.AddListener(UpdateSexiness);
        }

        private void OnPlayerCreated()
        {
            Sexiness = 0;
            Leanings = 0;
            Morals = 0;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TestAddPoints();
            }
        }

        public void UpdateMorals(int points)
        {
            Debug.Log("Update Morals " + points);
            Morals = Morals + points;
        }

        public void UpdateLeanings(int points)
        {
            Debug.Log("Update Leanings " + points);
            Leanings = Leanings + points;
        }

        public void UpdateSexiness(int points)
        {
            Debug.Log("Update Sexiness " + points);
            Sexiness = Sexiness + points;
        }

        public void TestAddPoints()
        {
            onAligneChanged.Invoke(5);
        }
    }
}
