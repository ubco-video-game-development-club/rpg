using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG;

public class AlignmentGraph : MonoBehaviour
{
    private RectTransform GraphContainer;

    [SerializeField] private Sprite point;

    private void Awake()
    {
        GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
    }

    private void Start()
    {
        GameManager.AlignmentSystem.OnAlignmentChanged.AddListener(ShowGraph);
    }

    private void CreatePoint(Vector2 anchoredPosition)
    {
        if (GameObject.Find("point") != null)
        {
            Destroy(GameObject.Find("point"));
        }

        GameObject NewPoint = new GameObject("point", typeof(Image));

        NewPoint.transform.SetParent(GraphContainer, false);
        NewPoint.GetComponent<Image>().sprite = point;

        RectTransform rectTransform = NewPoint.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void ShowGraph(float Morals, float Leanings, float Sexiness)
    {
        float xPosition = (Morals * 50) + 50;
        float yPosition = (Leanings * 50) + 50;

        CreatePoint(new Vector2(xPosition, yPosition));
    }
}
