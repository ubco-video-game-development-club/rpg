using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassNode
    {
        public int level;
        public ClassTierType nodeType;
        public List<int> childIndices;

        [NonSerialized] public bool isSelected;
        private Rect displayRect;

        public Vector2 KnobPosition { get; private set; }
        public Vector2 ButtonPosition { get; private set; }

        // Class
        public ClassBaseStats classBaseStats;

        // Subclass
        // TODO: after first development milestone

        // Skill
        public LevelUpOption[] levelUpOptions;

        public ClassNode(int level, ClassTierType nodeType)
        {
            this.level = level;
            this.nodeType = nodeType;
            childIndices = new List<int>();
        }

#if UNITY_EDITOR
        public void AddChild(int childIndex)
        {
            if (!childIndices.Contains(childIndex))
            {
                childIndices.Add(childIndex);
            }
        }

        public void RemoveChild(int childIndex)
        {
            childIndices.Remove(childIndex);
        }

        public void Draw(ClassTree tree, Vector2 position)
        {
            Color borderColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BORDER_COLOR;

            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;
            KnobPosition = new Vector2(position.x + nodeWidth / 2, position.y);
            ButtonPosition = new Vector2(position.x + nodeWidth / 2, position.y + nodeHeight);

            // Draw main node body
            displayRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
            EditorUtils.DrawBorderBox(displayRect, EditorUtils.BACKGROUND_COLOR, 2, borderColor);

            // Draw path connector knob
            if (level > 1)
            {
                Vector2 knobSize = Vector2.one * 8f;
                GUI.Box(new Rect(KnobPosition - knobSize / 2, knobSize), "", GUI.skin.button);
            }

            // Draw path edit button
            Vector2 buttonSize = Vector2.one * 12f;
            if (GUI.Button(new Rect(ButtonPosition - buttonSize / 2 - Vector2.up * 1, buttonSize), ""))
            {
                tree.StartPathEdit(this);
            }

            // Draw node label
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            string nodeText = EditorUtils.TrimStringToFit(nodeType.ToString(), nodeWidth, labelStyle);
            GUI.Label(displayRect, nodeText, labelStyle);
        }

        public void DrawPaths(ClassTree tree)
        {
            // Draw paths to children
            foreach (ClassNode child in tree.GetChildren(this))
            {
                Vector2 startPos = ButtonPosition;
                Vector2 endPos = child.KnobPosition;
                Handles.DrawBezier(startPos, endPos, startPos, endPos, EditorUtils.LINE_COLOR, null, 3f);
            }
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
#endif
    }
}
