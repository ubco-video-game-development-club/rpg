using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDebugText : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GetComponentInParent<Actor>().OnHealthChanged.AddListener(UpdateHealthText);
    }

    private void UpdateHealthText(int health)
    {
        text.text = health.ToString();
    }
}
