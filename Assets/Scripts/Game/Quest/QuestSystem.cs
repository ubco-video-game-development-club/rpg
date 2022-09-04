using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG
{
    public class QuestSystem : MonoBehaviour
    {
        public HashSet<Quest> Journal { get; private set; }

        private UnityEvent<Quest> onQuestAdded = new UnityEvent<Quest>();
        public UnityEvent<Quest> OnQuestAdded { get => onQuestAdded; }

        private UnityEvent<QuestNote> onQuestNoteAdded = new UnityEvent<QuestNote>();
        public UnityEvent<QuestNote> OnQuestNoteAdded { get => onQuestNoteAdded; }

        void Awake()
        {
            Journal = new HashSet<Quest>();
        }

        public void AddNote(QuestNote note)
        {
            Quest quest = note.Quest;
            AddQuest(quest);
            quest.AddNote(note);
            onQuestNoteAdded.Invoke(note);
        }

        public void AddQuest(Quest quest)
        {
            if (!Journal.Contains(quest))
            {
                Journal.Add(quest);
                quest.Initialize();
                onQuestAdded.Invoke(quest);
            }
        }
    }
}
