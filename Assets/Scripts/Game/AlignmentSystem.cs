using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace RPG
{
    public class AlignmentSystem : MonoBehaviour
    {
        private Player player;
        public UnityEvent<int> onAligneChanged;

        public int Sexiness
        {
            get => player.GetProperty<int>(PropertyName.Sexiness);
            set => player.SetProperty<int>(PropertyName.Sexiness, value);
        }

        public int Morals
        {
            get => player.GetProperty<int>(PropertyName.Morals);
            set => player.SetProperty<int>(PropertyName.Morals, value);
        }

        public int Leanings
        {
            get => player.GetProperty<int>(PropertyName.Leanings);
            set => player.SetProperty<int>(PropertyName.Leanings, value);
        }

        void Start()
        {
            onAligneChanged.AddListener(UpdateMorals);
            onAligneChanged.AddListener(UpdateLeanings);
            onAligneChanged.AddListener(UpdateSexiness);

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
