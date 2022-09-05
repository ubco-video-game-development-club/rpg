using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class DeathMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI deathText;

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Player.Respawn();
                SetVisible(false);
                enabled = false;
            }
        }

        public void SetDeathInfo(Actor killer)
        {
            deathText.text = "You were slain by <i>" + killer.DisplayName + "</i>";
            enabled = true;
        }
    }
}
