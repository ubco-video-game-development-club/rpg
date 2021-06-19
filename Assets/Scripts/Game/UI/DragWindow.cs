using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    private RectTransform parent;
    private Canvas canvas;

    void Awake()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        canvas = gameObject.GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData e)
    {
        parent.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnPointerClick(PointerEventData e)
    {
        parent.transform.SetAsLastSibling(); //This moves the window to the front (i.e. overtop other UI)
    }
}
