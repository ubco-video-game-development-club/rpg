using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class HUD : MonoBehaviour
    {
        private static HUD instance;

        [SerializeField] private RectTransform tooltipParent;
        public static RectTransform TooltipParent { get => instance.tooltipParent; }

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
    }
}
