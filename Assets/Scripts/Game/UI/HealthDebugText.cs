using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class HealthDebugText : MonoBehaviour
    {
        private TextMeshProUGUI text;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            GetComponentInParent<Actor>().AddPropertyChangedListener<int>(PropertyName.Health, UpdateHealthText);
        }

        private void UpdateHealthText(int health)
        {
            text.text = health.ToString();
        }
    }
}
