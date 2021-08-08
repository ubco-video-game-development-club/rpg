using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class QuestNote
    {
        public int QuestId { get; set; }
        public string Desc { get; set; }
    }

    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestNote> Notes { get; private set; }

        public Quest(int id, string title)
        {
            Id = id;
            Title = title;
            Notes = new List<QuestNote>();
        }

        public void AddNote(QuestNote note)
        {
            Notes.Add(note);
        }
    }

    public class QuestSystem : MonoBehaviour
    {
        public Dictionary<int, Quest> Quests { get; private set; }

        void Awake()
        {
            Quests = new Dictionary<int, Quest>();
            Quests.Add(0, new Quest(0, "My Quest 1"));
            Quests.Add(1, new Quest(1, "My Quest 2"));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                QuestNote note = new QuestNote();
                note.QuestId = 0;
                note.Desc = "This is a note for quest 1";
                AddQuestNote(note);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                QuestNote note = new QuestNote();
                note.QuestId = 1;
                note.Desc = "This is a note for quest 2";
                AddQuestNote(note);
            }
        }

        void OnGUI()
        {
            Rect layoutRect = new Rect(0, Screen.height / 2.0f, 200.0f, Screen.height / 2.0f);
            GUILayout.BeginArea(layoutRect);
            GUILayout.Label("Quest Notes");
            foreach (Quest quest in Quests.Values)
            {
                GUILayout.Label(quest.Title);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.BeginVertical();
                foreach (QuestNote note in quest.Notes)
                {
                    GUILayout.Label("- " + note.Desc);
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        public void AddQuestNote(QuestNote note)
        {
            Quests[note.QuestId].AddNote(note);
        }
    }
}
