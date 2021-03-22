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
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);

        public void BeginDialogue(Sprite portrait, string name, DialogueGraph graph)
        {
            dialogueUI.SetActive(true);
            dialoguePortrait.sprite = portrait;
            dialogueName.text = name;
            
            DialogueGraphNode entry = graph.nodes[0];
            StartCoroutine(ShowDialogue(entry.body));
        }

        private IEnumerator ShowDialogue(string dialogue)
        {
            int index = 0;
            while(index < dialogue.Length)
            {
                dialogueText.text = dialogue.Substring(0, index++);
                yield return letterCooldown;
            }
        }
    }
}
