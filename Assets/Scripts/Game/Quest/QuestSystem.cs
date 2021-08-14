using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class QuestSystem : MonoBehaviour
    {
        public HashSet<Quest> Journal { get; private set; }

        void Awake()
        {
            Journal = new HashSet<Quest>();
        }

        void OnGUI()
        {
            Rect layoutRect = new Rect(0, Screen.height / 2.0f, 200.0f, Screen.height / 2.0f);
            GUILayout.BeginArea(layoutRect);
            GUILayout.Label("Quest Notes");
            foreach (Quest quest in Journal)
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

        public void AddNote(QuestNote note)
        {
            Quest quest = note.Quest;
            AddQuest(quest);
            quest.AddNote(note);
        }

        public void AddQuest(Quest quest)
        {
            if (!Journal.Contains(quest))
            {
                Journal.Add(quest);
                quest.Initialize();
            }
        }
    }
}
