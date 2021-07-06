using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class ItemPickup : Interactable
    {
        public override void Interact(Player player)
        {
            Debug.Log("Interacted!");
        }
    }
}
