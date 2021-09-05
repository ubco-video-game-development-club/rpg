using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public class AlignmentSystem : MonoBehaviour
    {
        public UnityEvent<float, float, float> OnAlignmentChanged { get; private set; }

        private void Awake()
        {
            OnAlignmentChanged = new UnityEvent<float, float, float>();
        }

        private void Start()
        {
            Morals = 0;
            Leanings = 0;
            Sexiness = 0;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) && OnAlignmentChanged != null)
            {
                TestAddPoints();
            }
        }

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

        public void UpdateMorals(float points)
        {
            Debug.Log("Update Morals " + points);
            Morals += points;
            OnAlignmentChanged.Invoke(Morals, 0, 0);
        }

        public void UpdateLeanings(float points)
        {
            Debug.Log("Update Leanings " + points);
            Leanings += points;
            OnAlignmentChanged.Invoke(0, Leanings, 0);
        }

        public void UpdateSexiness(float points)
        {
            Debug.Log("Update Sexiness " + points);
            Sexiness += points;
            OnAlignmentChanged.Invoke(0, 0, points);
        }

        public void TestAddPoints()
        {
            UpdateMorals(.1f);
        }
    }
}
