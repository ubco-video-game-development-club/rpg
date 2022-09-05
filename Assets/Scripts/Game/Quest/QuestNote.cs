using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "New QuestNote", menuName = "Quest Note", order = 64)]
    public class QuestNote : ScriptableObject
    {
        [SerializeField] private Quest quest;
        public Quest Quest { get => quest; }

        [SerializeField][TextArea] private string desc;
        public string Desc { get => desc; }

        [SerializeField] private bool completed;
        public bool Completed { get => completed; }
    }
}
