using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG;
using BehaviourTree;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);
        private QuestGiver currentTarget;
        private DialogueGraph currentGraph;
        private int currentNode = 0;

        public void BeginDialogue(QuestGiver target, DialogueGraph graph)
        {
            currentTarget = target;
            currentGraph = graph;

            HUD.DialoguePanel.Show();
            HUD.DialoguePanel.SetTarget(target);

            StartCoroutine(ShowDialogue(0));
        }

        private DialogueGraphTransition[] GetTransitionsFor(DialogueGraph graph, int node)
        {
            LinkedList<DialogueGraphTransition> transitionsList = new LinkedList<DialogueGraphTransition>();
            foreach (DialogueGraphTransition t in graph.transitions)
            {
                if (t.from == node)
                {
                    transitionsList.AddLast(t);
                }
            }

            DialogueGraphTransition[] transitions = new DialogueGraphTransition[transitionsList.Count];
            transitionsList.CopyTo(transitions, 0);
            return transitions;
        }

        private IEnumerator ShowDialogue(int node)
        {
            currentNode = node;
            DialogueGraphNode graphNode = currentGraph.nodes[node];
            string dialogue = graphNode.body;
            HUD.DialoguePanel.PlayDialogue(dialogue);

            // Apply quest notes for this dialogue node after it finishes reading
            foreach (QuestNote note in graphNode.questNotes)
            {
                GameManager.QuestSystem.AddNote(note);
            }

            // Apply any dialogue index overrides
            foreach (DialogueIndexOverride idxOverride in graphNode.dialogueIndexOverrides)
            {
                QuestGiver target = currentTarget;
                if (idxOverride.targetUniqueID != "")
                {
                    target = Entity.Find<QuestGiver>(idxOverride.targetUniqueID);
                }
                target.ActiveIndex = idxOverride.indexOverride;
            }

            // Run custom dialogue behaviour
            if (graphNode.customBehaviour != null)
            {
                Tree<BehaviourTree.Behaviour>.Node root = graphNode.customBehaviour.Root;
                root.Element.Tick(root, HUD.DialoguePanel.GetComponent<BehaviourObject>());
            }

            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, node);
            foreach (DialogueGraphTransition t in transitions)
            {
                DialogueGraphNode to = t.to < 0 ? currentGraph.exitNode : currentGraph.nodes[t.to];
                HUD.DialoguePanel.CreateOption(to.name, (idx) => OnDialogueOptionClicked(idx));
                yield return letterCooldown;
            }
        }

        private void OnDialogueOptionClicked(int index)
        {
            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, currentNode);
            DialogueGraphTransition transition = transitions[index];
            if (transition.to < 0)
            {
                // We've reached the end of the tree
                HUD.DialoguePanel.Hide();
            }
            else
            {
                // Apply alignment modifications
                DialogueGraphNode graphNode = currentGraph.nodes[transition.to];
                GameManager.AlignmentSystem.UpdateLeanings(graphNode.leaningsMod);
                GameManager.AlignmentSystem.UpdateMorals(graphNode.moralsMod);
                GameManager.AlignmentSystem.UpdateSexiness(graphNode.sexinessMod);

                StartCoroutine(ShowDialogue(transition.to));
            }
        }
    }
}
