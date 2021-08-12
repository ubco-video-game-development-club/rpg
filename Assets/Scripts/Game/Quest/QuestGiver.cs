using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
    public class QuestGiver : Interactable
    {
        [SerializeField] private new string name;
        [SerializeField] private Sprite portrait;
        [SerializeField] private DialogueGraph dialogue;

        public override void Interact(Player player)
        {
            GameManager.DialogueSystem.BeginDialogue(portrait, name, dialogue);
        }
    }
}
