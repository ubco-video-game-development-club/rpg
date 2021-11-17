using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG;
using BehaviourTree;

namespace RPG
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private Image dialoguePortrait;
        [SerializeField] private TextMeshProUGUI dialogueName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform dialogueButtons;
        [SerializeField] private GameObject buttonPrefab;

        private List<GameObject> buttonPool = new List<GameObject>();
        private int buttonPoolIndex = 0;
        private YieldInstruction letterCooldown = new WaitForSeconds(0.05f);

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetTarget(NPC target)
        {
            dialoguePortrait.sprite = target.Portrait;
            dialogueName.text = target.CharacterName;
        }

        public void PlayDialogue(string dialogue)
        {
            StartCoroutine(AnimateDialogue(dialogue));
        }

        private IEnumerator AnimateDialogue(string dialogue)
        {
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
        }

        public void CreateOption(string name, UnityAction<int> listener)
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
            onButtonClicked.AddListener(() => listener(buttonIndex));
        }
    }
}
