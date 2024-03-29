using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG;
using Behaviours;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);
        private NPC currentTarget;
        private DialogueGraph currentGraph;
        private int currentNodeIdx = 0;

        public void BeginDialogue(NPC target, DialogueGraph graph)
        {
            // Disable actions of interacting entities
            if (GameManager.IsPlayerCreated)
            {
                GameManager.Player.SetInputEnabled(false);
            }

            if (target.TryGetComponent(out BehaviourObject obj))
            {
                obj.enabled = false;
            }

            currentTarget = target;
            currentGraph = graph;

            HUD.DialoguePanel.SetVisible(true);
            HUD.DialoguePanel.SetTarget(target);

            ShowDialogue(0);
        }

        private DialogueGraphTransition[] GetTransitionsFor(DialogueGraph graph, int nodeIdx)
        {
            LinkedList<DialogueGraphTransition> transitionsList = new LinkedList<DialogueGraphTransition>();
            foreach (DialogueGraphTransition t in graph.transitions)
            {
                if (t.from == nodeIdx)
                {
                    transitionsList.AddLast(t);
                }
            }

            DialogueGraphTransition[] transitions = new DialogueGraphTransition[transitionsList.Count];
            transitionsList.CopyTo(transitions, 0);
            return transitions;
        }

        private void ShowDialogue(int nodeIdx)
        {
            currentNodeIdx = nodeIdx;
            HUD.DialoguePanel.PlayDialogue(currentGraph.nodes[nodeIdx].body, () => OnDialogueNodeFinished(nodeIdx));
        }

        private void OnDialogueNodeFinished(int nodeIdx)
        {
            DialogueGraphNode graphNode = currentGraph.nodes[nodeIdx];
            string dialogue = graphNode.body;

            // Apply quest notes for this dialogue node after it finishes reading
            foreach (QuestNote note in graphNode.questNotes)
            {
                GameManager.QuestSystem.AddNote(note);
            }

            // Apply any dialogue index overrides
            foreach (DialogueIndexOverride idxOverride in graphNode.dialogueIndexOverrides)
            {
                NPC target = currentTarget;
                if (idxOverride.targetUniqueID != "")
                {
                    target = UniqueID.Get<NPC>(idxOverride.targetUniqueID);
                }
                target.ActiveIndex = idxOverride.indexOverride;
            }

            // Display transitions
            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, nodeIdx);
            foreach (DialogueGraphTransition t in transitions)
            {
                DialogueGraphNode to = t.to < 0 ? currentGraph.exitNode : currentGraph.nodes[t.to];
                HUD.DialoguePanel.CreateOption(to.name, (idx) => OnDialogueOptionClicked(idx));
            }
        }

        private void OnDialogueOptionClicked(int index)
        {
            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, currentNodeIdx);
            DialogueGraphTransition transition = transitions[index];

            // Run custom dialogue behaviour for prev node upon clicking any option
            DialogueGraphNode prevGraphNode = currentGraph.nodes[transition.from];
            if (prevGraphNode.customBehaviour != null)
            {
                Tree<Behaviours.Behaviour>.Node root = prevGraphNode.customBehaviour.Root;
                if (currentTarget.TryGetComponent<BehaviourObject>(out BehaviourObject obj))
                {
                    root.Element.Tick(root, obj, prevGraphNode);
                }
                else
                {
                    Debug.LogError("Failed to run custom dialogue behaviour due to target NPC not having a BehaviourObject component!");
                }
            }

            // Apply the selected transition
            if (transition.to < 0)
            {
                // We've reached the end of the tree
                HUD.DialoguePanel.SetVisible(false);

                if (GameManager.IsPlayerCreated)
                {
                    GameManager.Player.SetInputEnabled(true);
                }

                if (currentTarget.TryGetComponent(out BehaviourObject obj))
                {
                    obj.enabled = true;
                }
            }
            else
            {
                // Apply alignment modifications
                DialogueGraphNode graphNode = currentGraph.nodes[transition.to];
                GameManager.AlignmentSystem.UpdateLeanings(graphNode.leaningsMod);
                GameManager.AlignmentSystem.UpdateMorals(graphNode.moralsMod);
                GameManager.AlignmentSystem.UpdateSexiness(graphNode.sexinessMod);

                // Show the next dialogue node
                ShowDialogue(transition.to);
            }
        }
    }
}
