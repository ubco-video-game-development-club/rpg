using System;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 62)]
    public class Item : Upgrade
    {
        public static ItemSlot[] SlotTypes { get => (ItemSlot[])Enum.GetValues(typeof(ItemSlot)); }

        [SerializeField] private ItemPickup pickupPrefab;

        [SerializeField] private ItemSlot defaultSlot;
        public ItemSlot DefaultSlot { get => defaultSlot; }

        public void Drop(Vector2 position)
        {
            Instantiate(pickupPrefab, position, Quaternion.identity);
        }
    }
}
