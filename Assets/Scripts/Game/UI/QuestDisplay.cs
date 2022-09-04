using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG
{
    public class QuestDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI noteText;

        public void SetQuest(Quest quest)
        {
            titleText.text = quest.Title;

            string noteBody = "";
            foreach (QuestNote note in quest.Notes)
            {
                noteBody += "> " + note.Desc + "\n";
            }
            noteText.text = noteBody;
        }

        public float GetHeight()
        {
            noteText.ForceMeshUpdate();
            return titleText.GetComponent<RectTransform>().sizeDelta.y + noteText.textBounds.size.y;
        }
    }
}
