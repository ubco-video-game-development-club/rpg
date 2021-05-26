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

        public int MySexiness
        {
            get => player.GetProperty<int>(PropertyName.Sexiness);
            set => player.SetProperty<int>(PropertyName.Sexiness, value);
        }
        public int MyMorals
        {
            get => player.GetProperty<int>(PropertyName.Morals);
            set => player.SetProperty<int>(PropertyName.Morals, value);
        }
        public int MyLeanings
        {
            get => player.GetProperty<int>(PropertyName.Leanings);
            set => player.SetProperty<int>(PropertyName.Leanings, value);
        }

        void Start()
        {
            onAligneChanged.AddListener(UpdateMorals);
            onAligneChanged.AddListener(UpdateLeanings);
            onAligneChanged.AddListener(UpdateSexiness);

            MySexiness = 0;
            MyLeanings = 0;
            MyMorals = 0;
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
            MyMorals = MyMorals + points;
        }
        public void UpdateLeanings(int points)
        {
            Debug.Log("Update Leanings " + points);
            MyLeanings = MyLeanings + points;
        }
        public void UpdateSexiness(int points)
        {
            Debug.Log("Update Sexiness " + points);
            MySexiness = MySexiness + points;
        }
        public void TestAddPoints()
        {
            onAligneChanged.Invoke(5);
        }
    }
}
