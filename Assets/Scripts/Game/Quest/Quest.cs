using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 63)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string title;
        public string Title { get => title; }

        public HashSet<QuestNote> Notes { get; private set; }

        public void Initialize()
        {
            Notes = new HashSet<QuestNote>();
        }

        public void AddNote(QuestNote note)
        {
            if (!Notes.Contains(note))
            {
                Notes.Add(note);
            }
        }
    }
}
