using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Teleporter : Interactable
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float teleportTime;
        [SerializeField] private float fadeInTime;

        private WaitForSeconds fadeOutWait;
        private WaitForSeconds teleportWait;

        protected virtual void Awake()
        {
            fadeOutWait = new WaitForSeconds(fadeOutTime);
            teleportWait = new WaitForSeconds(teleportTime);
        }

        public override void Interact(Player player)
        {
            StartCoroutine(Teleport(player));
        }

        private IEnumerator Teleport(Player player)
        {
            player.SetInputEnabled(false);
            HUD.BlackScreen.SetVisible(true, fadeOutTime);

            yield return fadeOutWait;

            player.transform.position = spawnPoint.position;

            yield return teleportWait;

            HUD.BlackScreen.SetVisible(false, fadeInTime);
            player.SetInputEnabled(true);
        }
    }
}
