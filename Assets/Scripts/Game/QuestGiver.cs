using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
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
            GameManager.DialogueSystem.BeginDialogue(portrait, name, dialogue);
        }
    }
}
