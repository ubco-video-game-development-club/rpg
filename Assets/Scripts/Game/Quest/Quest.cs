using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 62)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string title;
        public string Title { get => title; }

        public List<QuestNote> Notes { get; private set; }

        public void AddNote(QuestNote note)
        {
            Notes.Add(note);
        }
    }
}
