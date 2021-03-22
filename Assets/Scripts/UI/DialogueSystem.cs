using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public void BeginDialogue(Sprite portrait, string name, DialogueGraph graph)
        {
            currentGraph = graph;

            dialogueUI.SetActive(true);
            dialoguePortrait.sprite = portrait;
            dialogueName.text = name;
            
            StartCoroutine(ShowDialogue(0));
        }

        private DialogueGraphNode[] GetTransitionsFor(DialogueGraph graph, int node)
        {
            LinkedList<DialogueGraphNode> transitionsList = new LinkedList<DialogueGraphNode>();
            foreach(DialogueGraphTransition t in graph.transitions)
            {
                if(t.from == node)
                {
                    DialogueGraphNode toNode;
                    if(t.to < 0) toNode = currentGraph.exitNode;
                    else toNode = graph.nodes[t.to];
                    transitionsList.AddLast(toNode);
                }
            }

            DialogueGraphNode[] transitions = new DialogueGraphNode[transitionsList.Count];
            transitionsList.CopyTo(transitions, 0);
            return transitions;
        }

        private IEnumerator ShowDialogue(int node)
        {
            DialogueGraphNode graphNode = currentGraph.nodes[node];
            string dialogue = graphNode.body;

            int index = 0;
            while(index < dialogue.Length)
            {
                dialogueText.text = dialogue.Substring(0, index++);
                yield return letterCooldown;
            }

            DialogueGraphNode[] transitions = GetTransitionsFor(currentGraph, node);
            foreach(DialogueGraphNode t in transitions)
            {
                CreateButton(t.name);
                yield return letterCooldown;
            }
        }

        private void CreateButton(string name)
        {
            if(buttonPoolIndex >= buttonPool.Count)
            {
                RectTransform button = Instantiate(buttonPrefab, Vector2.zero, Quaternion.identity, dialogueButtons).GetComponent<RectTransform>();
                button.anchoredPosition = Vector2.down * buttonPool.Count * 30;
                buttonPool.Add(button.gameObject);
                buttonPoolIndex++;
            } else 
            {
                GameObject button = buttonPool[buttonPoolIndex++];
                button.SetActive(true);
            }
        }
    }
}
