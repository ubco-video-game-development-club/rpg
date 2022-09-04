using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform fillBar;
        [SerializeField] private TextMeshProUGUI textbox;

        private void Awake()
        {
            if (GameManager.IsPlayerCreated) OnPlayerCreated();
            else GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            GameManager.Player.AddPropertyChangedListener<int>(PropertyName.Health, (health) => UpdateDisplay());
            GameManager.Player.AddPropertyChangedListener<int>(PropertyName.MaxHealth, (maxHealth) => UpdateDisplay());
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            int health = GameManager.Player.Health;
            int maxHealth = GameManager.Player.MaxHealth;
            fillBar.anchorMax = new Vector2((float)health / maxHealth, 1f);
            textbox.text = health + " / " + maxHealth;
        }
    }
}
