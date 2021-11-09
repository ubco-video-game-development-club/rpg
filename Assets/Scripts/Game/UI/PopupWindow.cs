using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class PopupWindow : MonoBehaviour
    {
        public string Title { get => title.text; set => title.SetText(value); }
        public Transform Content { get => content; }

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform content;

        public void OnClose() => Destroy(gameObject);
    }
}
