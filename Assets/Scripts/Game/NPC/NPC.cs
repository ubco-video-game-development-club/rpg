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

        public override bool ShowTooltip()
        {
            // TODO: fix this hack lol
            bool isBeeFighting = LevelManager.HasLevelProperty("isFighting") && (bool)LevelManager.GetLevelProperty("isFighting");
            bool isJimmyActive = LevelManager.HasLevelProperty("isJimmyActive") && (bool)LevelManager.GetLevelProperty("isJimmyActive");
            return LayerMask.LayerToName(gameObject.layer) == "Interactable" ||  (!isBeeFighting && !isJimmyActive);
        }

        // TODO: make enemy stuff based on factions!

        public void SetEnemy(bool isEnemy)
        {
            gameObject.layer = isEnemy ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("Interactable");
        }

        public bool IsEnemy()
        {
            return gameObject.layer == LayerMask.NameToLayer("Enemy");
        }
    }
}
