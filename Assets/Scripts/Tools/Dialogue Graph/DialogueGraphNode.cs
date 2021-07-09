using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueGraphNode
    {
        private static readonly Vector2 nodeSize = new Vector2(200, 50);

        public string name;
        public string body;
        public Vector2 position;

        private Rect displayRect;
        private bool isSelected = false;

        public DialogueGraphNode(string name, Vector2 position)
        {
            this.name = name;
        }

        public void Draw(Vector2 offset)
        {
            Vector2 pos = position - nodeSize / 2.0f;
            displayRect = new Rect(pos + offset, nodeSize);

            string displayName = EditorUtils.TrimStringToFit(name, displayRect.width, GUI.skin.box);
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