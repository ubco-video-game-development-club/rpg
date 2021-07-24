using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 62)]
    public class Item : Upgrade
    {
        public static ItemSlot[] SlotTypes { get => (ItemSlot[])Enum.GetValues(typeof(ItemSlot)); }

        [SerializeField] private ItemSlot slot;
        public ItemSlot Slot { get => slot; }
    }
}
