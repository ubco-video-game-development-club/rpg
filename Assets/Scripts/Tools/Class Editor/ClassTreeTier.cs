using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassTreeTier
    {
        public int level;
        public List<ClassTreeNode> nodes;

        public bool isSelected;
        private Rect displayRect;

        public ClassTreeTier(int level)
        {
            this.level = level;
            nodes = new List<ClassTreeNode>();
        }

        public void AddNode(ClassTreeNode node)
        {
            nodes.Add(node);
        }

        public void RemoveNode(ClassTreeNode node)
        {
            nodes.Remove(node);
        }

        public void Draw(Rect area)
        {
            displayRect = area;

            float tierheight = ClassTree.TIER_HEIGHT;
            float marginWidth = ClassTree.MARGIN_WIDTH;
            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;

            Color dividerColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BACKGROUND_COLOR;

            // Draw top divider
            EditorUtils.DrawBox(new Rect(area.x, area.y, area.width, 2), dividerColor);

            // Draw nodes
            int idx = 0;
            float sectionWidth = (area.width - marginWidth) / (nodes.Count + 1);
            float yOffset = (tierheight - nodeHeight) / 2 + 1;
            foreach (ClassTreeNode node in nodes)
            {
                node.Draw(new Vector2(area.x + marginWidth + sectionWidth * (idx + 1) - nodeWidth / 2, area.y + yOffset));
                idx++;
            }

            // Draw level label
            GUI.Label(new Rect(area.x + marginWidth - 60, area.y + area.height / 2 - 10, 60, 20), $"Level {level}");

            // Draw bottom divider
            EditorUtils.DrawBox(new Rect(area.x, area.y + area.height, area.width, 2), dividerColor);
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
