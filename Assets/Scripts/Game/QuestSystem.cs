using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class QuestSystem : MonoBehaviour
    {
        public int NoteCount { get => questNotes.Count; }

        //TODO: Quests
        [SerializeField] private GameObject notesPrefab;
        private List<Note> questNotes;
        private GameObject notesPopup;

    #if UNITY_EDITOR
        private string noteName = "Note Name";
        private string noteContent = "Note Content";
    #endif

        void Awake()
        {
            questNotes = new List<Note>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Toggle Notes"))
            {
                if (notesPopup == null) notesPopup = GameManager.PopupSystem.CreatePopup("Notes", notesPrefab);
                else Destroy(notesPopup);
            }
        }

    #if UNITY_EDITOR
        void OnGUI()
        {
            Rect layoutRect = new Rect(0, Screen.height / 2.0f, 200.0f, Screen.height / 2.0f);
            GUILayout.BeginArea(layoutRect);
            GUILayout.Label("Quest Notes");
            noteName = GUILayout.TextField(noteName);
            noteContent = GUILayout.TextArea(noteContent, GUILayout.Height(100));

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Note"))
            {
                AddNote(noteName, noteContent);
                noteName = "";
                noteContent = "";
            }
            
            if(GUILayout.Button("Remove Note")) RemoveNote(noteName);
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    #endif

        public void AddNote(string name, string content)
        {
            questNotes.Add(new Note()
            {
                name = name,
                content = content
            });
        }

        public Note GetNote(int index) => questNotes[index];

        public bool RemoveNote(Note note) => questNotes.Remove(note);

        public bool RemoveNote(string name)
        {
            Note? remove = null;
            foreach (Note note in questNotes)
            {
                if (note.name == name)
                {
                    remove = note;
                    break;
                }
            }

            if (remove == null) return false;
            else return questNotes.Remove(remove ?? default(Note));
        }

        public struct Note
        {
            public string name;
            public string content;
        }
    }
}
