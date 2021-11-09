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

        [SerializeField] private RectTransform popupParent;
        public static RectTransform PopupParent { get => instance.popupParent; }

        [SerializeField] private DialoguePanel dialoguePanel;
        public static DialoguePanel DialoguePanel { get => instance.dialoguePanel; }

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
