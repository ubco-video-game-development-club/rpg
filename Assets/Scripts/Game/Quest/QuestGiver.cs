using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG
{
    public class QuestGiver : Interactable
    {
        [SerializeField] private string characterName;
        public string CharacterName { get => characterName; }

        [SerializeField] private Sprite portrait;
        public Sprite Portrait { get => portrait; }

        [SerializeField] private DialogueGraph[] dialogueGraphs;

        private int currentIndex = 0;

        public override void Interact(Player player)
        {
            DialogueGraph graph = dialogueGraphs[currentIndex];
            GameManager.DialogueSystem.BeginDialogue(this, graph);
        }

        public void SetActiveIndex(int idx)
        {
            currentIndex = idx;
        }
    }
}
