using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class PopupSystem : MonoBehaviour
    {
        [SerializeField] private PopupWindow popupPrefab;

        public GameObject CreatePopup(string title, GameObject content)
        {
            PopupWindow window = Instantiate(popupPrefab, HUD.PopupParent);
            window.Title = title;

            Instantiate(content, window.Content);
            return window.gameObject;
        }
    }
}
