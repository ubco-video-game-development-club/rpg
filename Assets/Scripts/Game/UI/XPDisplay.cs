using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class XPDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform fillBar;
        [SerializeField] private TextMeshProUGUI textbox;

        private void Start()
        {
            GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            GameManager.Player.AddPropertyChangedListener<int>(PropertyName.XP, (xp) => UpdateDisplay());
            GameManager.Player.AddPropertyChangedListener<int>(PropertyName.LevelUps, (levelUps) => UpdateDisplay());
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            int xp = GameManager.LevelingSystem.XP;
            int xpToLevel = GameManager.LevelingSystem.GetRequiredXP();
            int potentialLevel = GameManager.LevelingSystem.GetPotentialLevel();
            fillBar.anchorMax = new Vector2((float)xp / xpToLevel, 1f);
            textbox.text = xp + " / " + xpToLevel + " XP to Level " + potentialLevel;
        }
    }
}
