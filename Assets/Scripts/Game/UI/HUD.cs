using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        private int maxHealth;
        private int health;

        void Start()
        {
            Player player = GameManager.Player;

            //Initialize UI
            OnMaxHealthChanged(player.GetProperty<int>(PropertyName.MaxHealth));
            OnHealthChanged(player.GetProperty<int>(PropertyName.Health));

            //Hook events
            player.AddPropertyChangedListener<int>(PropertyName.MaxHealth, OnMaxHealthChanged);
            player.AddPropertyChangedListener<int>(PropertyName.Health, OnHealthChanged);
        }

        private void OnMaxHealthChanged(int value)
        {
            maxHealth = value;
            UpdateUI();
        }

        private void OnHealthChanged(int value)
        {
            health = value;
            UpdateUI();
        }

        private void UpdateUI()
        {
            float fillAmount = (float)health / (float)maxHealth;
            healthBar.fillAmount = fillAmount;
        }
    }
}
