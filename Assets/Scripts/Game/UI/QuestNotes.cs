using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class QuestNotes : MonoBehaviour
    {
        [SerializeField] private RectTransform notesListContent;
        [SerializeField] private TMPro.TextMeshProUGUI noteContent;
        [SerializeField] private Button noteSelectButton;

        void Start()
        {
            QuestSystem questSystem = GameManager.QuestSystem;
            int noteCount = questSystem.NoteCount;
            float buttonHeight = noteSelectButton.GetComponent<RectTransform>().sizeDelta.y + 5.0f;
            notesListContent.sizeDelta = new Vector2(notesListContent.sizeDelta.x, noteCount * buttonHeight + 10.0f);

            for(int i = 0; i < noteCount; i++)
            {
                QuestSystem.Note note = questSystem.GetNote(i);
                Button button = Instantiate(noteSelectButton, notesListContent);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -(buttonHeight * i + 10.0f));
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(note.name);
                
                int index = i;
                button.onClick.AddListener(delegate
                {
                    SelectNote(index);
                });
            }

            if(noteCount > 0) SelectNote(0);
        }

        private void SelectNote(int index)
        {
            QuestSystem.Note note = GameManager.QuestSystem.GetNote(index);
            noteContent.SetText(note.content);
        }
    }
}
