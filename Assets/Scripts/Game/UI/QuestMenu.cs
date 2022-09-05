using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class QuestMenu : Menu
    {
        [SerializeField] private QuestDisplay questPrefab;

        private List<QuestDisplay> questDisplays = new List<QuestDisplay>();

        private RectTransform rectTransform;

        protected override void Awake()
        {
            base.Awake();

            rectTransform = GetComponent<RectTransform>();

            if (GameManager.IsPlayerCreated) OnPlayerCreated();
            else GameManager.AddPlayerCreatedListener(OnPlayerCreated);
        }

        private void OnPlayerCreated()
        {
            GameManager.QuestSystem.OnQuestAdded.AddListener((quest) => UpdateDisplay());
            GameManager.QuestSystem.OnQuestNoteAdded.AddListener((questNote) => UpdateDisplay());
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            foreach (QuestDisplay oldDisplay in questDisplays)
            {
                Destroy(oldDisplay.gameObject);
            }

            questDisplays.Clear();
            float yOffset = 0;

            foreach (Quest quest in GameManager.QuestSystem.Journal)
            {
                if (quest.IsCompleted()) continue;

                QuestDisplay questDisplay = Instantiate(questPrefab, transform);
                questDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -yOffset);
                questDisplay.SetQuest(quest);
                questDisplays.Add(questDisplay);

                yOffset += questDisplay.GetHeight() + 10f;
            }

            SetVisible(yOffset > 0);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, yOffset);
        }
    }
}
