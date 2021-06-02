using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassNode
    {
        public bool isSelected;
        private Rect displayRect;

        public void Draw(Vector2 position)
        {
            Color borderColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BORDER_COLOR;

            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;
            displayRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
            EditorUtils.DrawBorderBox(displayRect, EditorUtils.BACKGROUND_COLOR, 1, borderColor);
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
