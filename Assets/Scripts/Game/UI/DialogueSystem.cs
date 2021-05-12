using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private Image dialoguePortrait;
        [SerializeField] private TMPro.TextMeshProUGUI dialogueName;
        [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
        [SerializeField] private Transform dialogueButtons;
        [SerializeField] private GameObject buttonPrefab;
        private List<GameObject> buttonPool = new List<GameObject>();
        private int buttonPoolIndex = 0;
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);
        private DialogueGraph currentGraph;
        private int currentNode = 0;

        public void BeginDialogue(Sprite portrait, string name, DialogueGraph graph)
        {
            currentGraph = graph;

            dialogueUI.SetActive(true);
            dialoguePortrait.sprite = portrait;
            dialogueName.text = name;
            
            StartCoroutine(ShowDialogue(0));
        }

        private DialogueGraphTransition[] GetTransitionsFor(DialogueGraph graph, int node)
        {
            LinkedList<DialogueGraphTransition> transitionsList = new LinkedList<DialogueGraphTransition>();
            foreach(DialogueGraphTransition t in graph.transitions)
            {
                if(t.from == node)
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

            foreach(GameObject go in buttonPool)
            {
                go.SetActive(false);
            }
            buttonPoolIndex = 0;

            int index = 0;
            while(index <= dialogue.Length)
            {
                dialogueText.text = dialogue.Substring(0, index++);
                yield return letterCooldown;
            }

            DialogueGraphTransition[] transitions = GetTransitionsFor(currentGraph, node);
            foreach(DialogueGraphTransition t in transitions)
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
            if(transition.to < 0) dialogueUI.SetActive(false);
            else StartCoroutine(ShowDialogue(transition.to));
        }

        private void CreateButton(string name)
        {
            TMPro.TextMeshProUGUI buttonText;
            UnityEvent onButtonClicked;

            int buttonIndex = buttonPoolIndex;
            if(buttonPoolIndex >= buttonPool.Count)
            {
                RectTransform button = Instantiate(buttonPrefab, Vector2.zero, Quaternion.identity, dialogueButtons).GetComponent<RectTransform>();
                button.anchoredPosition = Vector2.down * buttonPool.Count * 30;
                onButtonClicked = button.GetComponent<Button>().onClick;
                buttonText = button.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

                buttonPool.Add(button.gameObject);
                buttonPoolIndex++;
            } else 
            {
                GameObject button = buttonPool[buttonPoolIndex++];
                button.SetActive(true);
                onButtonClicked = button.GetComponent<Button>().onClick;
                buttonText = button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
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
