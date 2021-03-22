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
            }
        }

        private void CreateButton(string name)
        {
            Debug.Log($"Create button with name {name}");
        }
    }
}
