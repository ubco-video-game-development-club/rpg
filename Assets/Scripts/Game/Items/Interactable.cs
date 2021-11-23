using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private Tooltip tooltipPrefab;
        [SerializeField] [TextArea] private string interactHint;

        private Tooltip tooltip;

        void Start()
        {
            tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
            tooltip.SetTarget(transform);
            tooltip.SetText("[E] " + interactHint);
            tooltip.SetActive(false);
        }

        void OnDestroy()
        {
            if (tooltip != null)
            {
                Destroy(tooltip.gameObject);
            }
        }

        public void SetTooltipActive(bool active)
        {
            tooltip.SetActive(active);
        }

        public abstract void Interact(Player player);
    }
}
