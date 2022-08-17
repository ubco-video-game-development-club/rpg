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

        [SerializeField] private Menu blackScreen;
        public static Menu BlackScreen { get => instance.blackScreen; }

        [SerializeField] private DialogueMenu dialoguePanel;
        public static DialogueMenu DialoguePanel { get => instance.dialoguePanel; }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            if (!GameManager.IsPlayerCreated)
            {
                HUD.BlackScreen.SetVisible(true);
                GameManager.AddPlayerCreatedListener(OnPlayerCreated);
            }
        }

        private void OnPlayerCreated()
        {
            HUD.BlackScreen.SetVisible(false);
        }
    }
}
