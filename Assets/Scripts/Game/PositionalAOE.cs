using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PositionalAOE", menuName = "Actions/PositionalAOE", order = 50)]
public class PositionalAOE : Action
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 1;
    [SerializeField] private float radius = 1;

    public override void Invoke()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(effectPrefab, mousePos, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, radius);
        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.name);
        }
    }
}
