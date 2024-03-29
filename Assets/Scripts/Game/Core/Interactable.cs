using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private Tooltip tooltipPrefab;
        [SerializeField] private string usableLayer = "Interactable";
        [SerializeField] private string disabledLayer = "Default";
        [SerializeField][TextArea] private string interactHint;

        private Tooltip tooltip;

        protected virtual void Start()
        {
            tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity, HUD.TooltipParent);
            tooltip.SetTarget(transform);
            tooltip.SetText("[E] " + interactHint);
            tooltip.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            if (tooltip != null)
            {
                Destroy(tooltip.gameObject);
            }
        }

        public virtual bool ShowTooltip()
        {
            return true;
        }

        public void SetUsable(bool usable)
        {
            gameObject.layer = LayerMask.NameToLayer(usable ? usableLayer : disabledLayer);
        }

        public void SetTooltipActive(bool active)
        {
            tooltip.SetActive(active);
        }

        public abstract void Interact(Player player);
    }
}
