using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class AlignmentGraph : MonoBehaviour
    {
        [SerializeField] private RectTransform point;

        private void Start()
        {
            GameManager.AlignmentSystem.OnAlignmentChanged.AddListener(UpdatePoint);
        }

        private void UpdatePoint(float Morals, float Leanings, float Sexiness)
        {
            float xPosition = (Morals * 50) + 50;
            float yPosition = (Leanings * 50) + 50;
            point.anchoredPosition = new Vector2(xPosition, yPosition);
        }
    }
}
