using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueGraphNode : EditorNode
    {
        private static readonly Vector2 nodeSize = new Vector2(200, 50);

        public string name;
        public string body;

        public DialogueGraphNode(string name, Vector2 position) : base(position)
        {
            this.name = name;
        }

        public override void Draw(Vector2 offset)
        {
            Vector2 pos = position - nodeSize / 2.0f;
            displayRect = new Rect(pos + offset, nodeSize);

            string displayName = EditorUtils.TrimStringToFit(name, displayRect.width, GUI.skin.box);
            GUI.Box(displayRect, displayName, GUI.skin.button);
        }
    }
}