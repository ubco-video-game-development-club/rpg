using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class ItemPickup : Interactable
    {
        [SerializeField] private Item item;

        public override void Interact(Player player)
        {
            player.Equip(item.Slot, item);
            Destroy(gameObject);
        }
    }
}
