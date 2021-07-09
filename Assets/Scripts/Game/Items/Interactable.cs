using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class Interactable : Entity
    {
        [SerializeField] private Tooltip tooltipPrefab;
        [SerializeField] [TextArea] private string interactHint;

        private Tooltip tooltip;

        void Start()
        {
            tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
            tooltip.SetTarget(transform);
            tooltip.SetText(interactHint);
            tooltip.SetActive(false);
        }

        public void SetTooltipActive(bool active)
        {
            tooltip.SetActive(active);
        }

        public abstract void Interact(Player player);
    }
}
