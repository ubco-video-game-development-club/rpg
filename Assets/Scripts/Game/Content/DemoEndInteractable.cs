using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class DemoEndInteractable : Interactable
    {
        public override void Interact(Player player)
        {
            player.SetInputEnabled(false);
            HUD.DemoEndScreen.SetDemoEnd();
            HUD.DemoEndScreen.SetVisible(true);
        }
    }
}
