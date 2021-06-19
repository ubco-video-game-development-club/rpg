using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG
{
    public class QuestNotes : MonoBehaviour
    {
        [SerializeField] private RectTransform notesListContent;
        [SerializeField] private TextMeshProUGUI noteContent;
        [SerializeField] private Button noteSelectButton;
        private float buttonHeight;

        void Awake()
        {
            buttonHeight = noteSelectButton.GetComponent<RectTransform>().sizeDelta.y + 5.0f;
        }

        void Start()
        {
            QuestSystem questSystem = GameManager.QuestSystem;
            questSystem.OnQuestNoteAdd.AddListener(OnNoteAdded);
            questSystem.OnQuestNoteRemove.AddListener(OnNoteRemoved);

            SpawnNotes();
            if(questSystem.NoteCount > 0) SelectNote(0);
        }

        void OnDestroy()
        {
            QuestSystem questSystem = GameManager.QuestSystem;
            questSystem.OnQuestNoteAdd.RemoveListener(OnNoteAdded);
            questSystem.OnQuestNoteRemove.RemoveListener(OnNoteRemoved);
        }

        private void OnNoteAdded(QuestSystem.Note note) => SpawnNote(note, GameManager.QuestSystem.NoteCount - 1);

        private void OnNoteRemoved(QuestSystem.Note note)
        {
            for(int i = 0; i < notesListContent.childCount; i++)
            {
                Transform child = notesListContent.GetChild(i);
                Destroy(child.gameObject);
            }

            SpawnNotes();
        }

        private void SelectNote(int index)
        {
            QuestSystem.Note note = GameManager.QuestSystem.GetNote(index);
            noteContent.SetText(note.content);
        }

        private void SpawnNotes()
        {
            QuestSystem questSystem = GameManager.QuestSystem;
            int noteCount = questSystem.NoteCount;
            notesListContent.sizeDelta = new Vector2(notesListContent.sizeDelta.x, noteCount * buttonHeight + 10.0f);

            for (int i = 0; i < noteCount; i++)
            {
                QuestSystem.Note note = questSystem.GetNote(i);
                SpawnNote(note, i);
            }
        }

        private void SpawnNote(QuestSystem.Note note, int index)
        {
            Button button = Instantiate(noteSelectButton, notesListContent);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -(buttonHeight * index + 10.0f));
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(note.name);

            button.onClick.AddListener(delegate
            {
                SelectNote(index);
            });
        }
    }
}
