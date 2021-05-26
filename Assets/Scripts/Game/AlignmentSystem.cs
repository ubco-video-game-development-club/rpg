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
        [SerializeField] private float Morals = 0F;
        [SerializeField] private float Leanings = 0F;
        [SerializeField] private float Sexiness = 0F;

        // Add new property names to PropertyName.cs
        private int MyHealth
        {
            get => player.GetProperty<int>(PropertyName.Health);
            set => player.SetProperty<int>(PropertyName.Health, value);
        }

        private Player player;

        void Start()
        {
            player = GameManager.Player;

            onAligneChanged.AddListener(UpdateMorals);
            onAligneChanged.AddListener(UpdateLeaning);
            onAligneChanged.AddListener(UpdateSexiness);

            // Example code (old)
            player.SetProperty<int>(PropertyName.Health, 3);
            int health = player.GetProperty<int>(PropertyName.Health);

            // Example code (new)
            MyHealth = 3;
            int tmpHealth = MyHealth;
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
            //Morals = Morals + points;
        }
        public void UpdateLeaning(int points)
        {
            Debug.Log("Update Leanings " + points);
            //Leanings = Leanings + points;
        }
        public void UpdateSexiness(int points)
        {
            Debug.Log("Update Sexiness " + points);
            //Sexiness = Sexiness + points;
        }
        public void TestAddPoints()
        {
            onAligneChanged.Invoke(5);
        }
    }
}
