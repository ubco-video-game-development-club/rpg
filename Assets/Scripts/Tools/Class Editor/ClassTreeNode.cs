using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassTreeNode
    {
        public bool unlocked;
        public bool taken;

        /// Editor Functions

        private Rect displayRect;

        public void Draw(Vector2 position)
        {
            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;
            displayRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
            EditorUtils.DrawBorderBox(displayRect, EditorUtils.HEADER_COLOR, 1, EditorUtils.DIVIDER_COLOR);
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
