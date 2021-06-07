using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace ClassEditor
{
    public enum ClassNodeType
    {
        Subclass, Skill
    }

    [System.Serializable]
    public class ClassNode
    {
        public bool isSelected;
        private Rect displayRect;

        public ClassNodeType nodeType;
        public List<int> childIndices;

        // Subclasses
        // TODO: after first development milestone

        // Skills
        public LevelUpOption[] levelUpOptions;

        public ClassNode(ClassNodeType nodeType)
        {
            this.nodeType = nodeType;
        }

        public void Draw(Vector2 position)
        {
            Color borderColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BORDER_COLOR;

            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;
            displayRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
            EditorUtils.DrawBorderBox(displayRect, EditorUtils.BACKGROUND_COLOR, 1, borderColor);

            if (nodeType == ClassNodeType.Skill)
            {
                GUIStyle nameStyle = new GUIStyle(GUI.skin.label);
                nameStyle.alignment = TextAnchor.MiddleCenter;
                string nameText = EditorUtils.TrimStringToFit("Skill", nodeWidth, nameStyle);
                GUI.Label(displayRect, nameText, nameStyle);
            }
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
