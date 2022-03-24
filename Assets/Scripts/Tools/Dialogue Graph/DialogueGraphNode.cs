using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueGraphNode
    {
        private static readonly Vector2 nodeSize = new Vector2(200, 50);

        public string name;
        public string displayName;
        public string body;
        public Vector2 position;
        [Range(-1.0f, 1.0f)] public float moralsMod;
        [Range(-1.0f, 1.0f)] public float leaningsMod;
        [Range(-1.0f, 1.0f)] public float sexinessMod;
        public QuestNote[] questNotes;
        [Tooltip("Overrides the currently active dialogue graph index for the target entity (using UniqueID).")]
        public DialogueIndexOverride[] dialogueIndexOverrides;
        public BehaviourTree.BehaviourTree customBehaviour;

        private Rect displayRect;
        private bool isSelected = false;

        public DialogueGraphNode(string name, Vector2 position)
        {
            this.name = name;
            this.position = position;
            displayName = null;
        }

        public void Draw(Vector2 offset)
        {
            if (string.IsNullOrWhiteSpace(displayName)) displayName = EditorUtils.TrimStringToFit(name, nodeSize.x, GUI.skin.box);

            Vector2 pos = position - nodeSize / 2.0f;
            displayRect = new Rect(pos + offset, nodeSize);
            GUI.Box(displayRect, displayName, GUI.skin.button);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && displayRect.Contains(e.mousePosition))
                    {
                        isSelected = true;
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        isSelected = false;
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (isSelected)
                    {
                        position += e.delta;
                        GUI.changed = true;
                    }
                    break;
                default:
                    break;
            }

            return isSelected;
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}