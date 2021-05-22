using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private PopupWindow popupPrefab;

    public void CreatePopup(string title, GameObject content)
    {
        PopupWindow window = Instantiate(popupPrefab, canvas);
        window.Title = title;

        Instantiate(content, window.Content);
    }
}
