using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RPG;
using Behaviours;

namespace RPG
{
    public class DialogueMenu : Menu
    {
        [SerializeField] private Image dialoguePortrait;
        [SerializeField] private TextMeshProUGUI dialogueName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform dialogueButtons;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private string defaultOption = "[...]";
        [SerializeField] private float letterDelay = 0.05f;
        [SerializeField] private float commaDelay = 0.2f;
        [SerializeField] private float periodDelay = 0.5f;

        private List<GameObject> buttonPool = new List<GameObject>();
        private int buttonPoolIndex = 0;
        private WaitForSeconds letterWait;
        private WaitForSeconds commaWait;
        private WaitForSeconds periodWait;
        private bool skipDialogue = false;

        protected override void Awake()
        {
            base.Awake();
            letterWait = new WaitForSeconds(letterDelay);
            commaWait = new WaitForSeconds(commaDelay);
            periodWait = new WaitForSeconds(periodDelay);
        }

        private void Update()
        {
            if (Input.GetButtonUp("SkipDialogue")) skipDialogue = true;
        }

        public void SetTarget(NPC target)
        {
            dialoguePortrait.sprite = target.Portrait;
            dialogueName.text = target.CharacterName;
        }

        public void PlayDialogue(string dialogue, UnityAction onFinished)
        {
            skipDialogue = false;
            StartCoroutine(AnimateDialogue(dialogue, onFinished));
        }

        private IEnumerator AnimateDialogue(string dialogue, UnityAction onFinished)
        {
            foreach (GameObject go in buttonPool)
            {
                go.SetActive(false);
            }
            buttonPoolIndex = 0;

            int index = 0;
            while (index < dialogue.Length)
            {
                if (skipDialogue)
                {
                    break;
                }

                dialogueText.text = dialogue.Substring(0, index + 1);

                WaitForSeconds currWait = letterWait;
                string currLetter = dialogue.Substring(index, 1);
                string nextLetter = index + 1 < dialogue.Length ? dialogue.Substring(index + 1, 1) : "";

                if (currLetter == "," || currLetter == "-")
                {
                    currWait = commaWait;
                }
                else if ((currLetter == "." || currLetter == "!" || currLetter == "?") && nextLetter == " ")
                {
                    currWait = periodWait;
                }

                yield return currWait;

                index++;
            }

            dialogueText.text = dialogue;
            skipDialogue = false;

            onFinished();
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

            buttonText.SetText(name.Length > 0 ? name : defaultOption);
            onButtonClicked.RemoveAllListeners();
            onButtonClicked.AddListener(() => listener(buttonIndex));
        }
    }
}
