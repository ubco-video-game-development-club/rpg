using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Vector2 offset;

        private Transform target;
        private CanvasGroup canvasGroup;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (target == null) return;
            transform.position = Camera.main.WorldToScreenPoint((Vector2)target.position + offset);
        }

        public void SetActive(bool active)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetText(string text)
        {
            textField.text = text;
        }
    }
}
