using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite portrait;
    [SerializeField] private DialogueGraph dialogue;

    void OnMouseDown()
    {
        Debug.LogWarning("This is for testing. Please fix me.");
        Interact();
    }

    public void Interact()
    {
        DialogueSystem dialogueSystem = GameManager.Instance.DialogueSystem;
        dialogueSystem.BeginDialogue(portrait, name, dialogue);
    }
}
