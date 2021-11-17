using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
    public class NPC : Interactable
    {
        [SerializeField] private string characterName;
        public string CharacterName { get => characterName; }

        [SerializeField] private Sprite portrait;
        public Sprite Portrait { get => portrait; }

        [SerializeField] private DialogueGraph[] dialogueGraphs;

        public int ActiveIndex { get; set; }

        public override void Interact(Player player)
        {
            DialogueGraph graph = dialogueGraphs[ActiveIndex];
            GameManager.DialogueSystem.BeginDialogue(this, graph);
        }
    }
}
