using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private PopupWindow popupPrefab;

    public GameObject CreatePopup(string title, GameObject content)
    {
        PopupWindow window = Instantiate(popupPrefab, canvas);
        window.Title = title;

        Instantiate(content, window.Content);
        return window.gameObject;
    }

    public GameObject CreatePopup(string title, out Transform content)
    {
        PopupWindow window = Instantiate(popupPrefab, canvas);
        window.Title = title;
        content = window.Content.transform;
        return window.gameObject;
    }
}
