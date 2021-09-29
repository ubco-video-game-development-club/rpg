using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private Image dialoguePortrait;
        [SerializeField] private TextMeshProUGUI dialogueName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform dialogueButtons;
        [SerializeField] private GameObject buttonPrefab;

        private List<GameObject> buttonPool = new List<GameObject>();
        private int buttonPoolIndex = 0;
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);
        private QuestGiver currentTarget;
        private DialogueGraph currentGraph;
        private int currentNode = 0;

        public void BeginDialogue(QuestGiver target, DialogueGraph graph)
        {
            currentTarget = target;
            currentGraph = graph;

            dialogueUI.SetActive(true);
            dialoguePortrait.sprite = target.Portrait;
            dialogueName.text = target.CharacterName;

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

            foreach (GameObject go in buttonPool)
            {
                go.SetActive(false);
            }
            buttonPoolIndex = 0;

            int index = 0;
            while (index <= dialogue.Length)
            {
                dialogueText.text = dialogue.Substring(0, index++);
                yield return letterCooldown;
            }

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
                    target = Entity.Find(idxOverride.targetUniqueID).GetComponent<QuestGiver>();
                }
                target.SetActiveIndex(idxOverride.indexOverride);
            }

            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, node);
            foreach (DialogueGraphTransition t in transitions)
            {
                DialogueGraphNode to = t.to < 0 ? currentGraph.exitNode : currentGraph.nodes[t.to];
                CreateButton(to.name);
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
                dialogueUI.SetActive(false);
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

        private void CreateButton(string name)
        {
            TextMeshProUGUI buttonText;
            UnityEvent onButtonClicked;

            int buttonIndex = buttonPoolIndex;
            if (buttonPoolIndex >= buttonPool.Count)
            {
                RectTransform button = Instantiate(buttonPrefab, Vector2.zero, Quaternion.identity, dialogueButtons).GetComponent<RectTransform>();
                button.anchoredPosition = Vector2.down * buttonPool.Count * 30;
                onButtonClicked = button.GetComponent<Button>().onClick;
                buttonText = button.GetChild(0).GetComponent<TextMeshProUGUI>();

                buttonPool.Add(button.gameObject);
                buttonPoolIndex++;
            }
            else
            {
                GameObject button = buttonPool[buttonPoolIndex++];
                button.SetActive(true);
                onButtonClicked = button.GetComponent<Button>().onClick;
                buttonText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            }

            buttonText.SetText(name);
            onButtonClicked.RemoveAllListeners();
            onButtonClicked.AddListener(delegate
            {
                OnDialogueOptionClicked(buttonIndex);
            });
        }
    }
}
