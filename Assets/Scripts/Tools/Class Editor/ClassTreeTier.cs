using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassTreeTier
    {
        public int Level { get; private set; }
        public List<ClassTreeNode> Nodes { get; private set; }

        public ClassTreeTier(int level)
        {
            Level = level;
            Nodes = new List<ClassTreeNode>();
        }

        public void AddNode(ClassTreeNode node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(ClassTreeNode node)
        {
            Nodes.Remove(node);
        }

        /// Editor Functions

        private Rect displayRect;

        public void Draw(Rect area)
        {
            displayRect = area;

            float tierheight = ClassTree.TIER_HEIGHT;
            float marginWidth = ClassTree.MARGIN_WIDTH;
            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;

            // Draw top divider
            EditorUtils.DrawBox(new Rect(area.x, area.y, area.width, 2), EditorUtils.HEADER_COLOR);

            // Draw nodes
            int idx = 0;
            float sectionWidth = (area.width - marginWidth) / (Nodes.Count + 1);
            float yOffset = (tierheight - nodeHeight) / 2 + 1;
            foreach (ClassTreeNode node in Nodes)
            {
                node.Draw(new Vector2(area.x + sectionWidth * (idx + 1) - nodeWidth / 2, area.y + yOffset));
                idx++;
            }

            // TODO: Move this back to class tree, and move the margin to the left side
            // Draw vertical divider
            EditorUtils.DrawBox(new Rect(area.width - marginWidth + 1, area.y, 2, area.height), EditorUtils.DIVIDER_COLOR);

            // Draw level label
            GUI.Label(new Rect(area.width - marginWidth + 20, area.y + area.height / 2 - 15, 80, 30), $"Level {Level}");

            // Draw bottom divider
            EditorUtils.DrawBox(new Rect(area.x, area.y + area.height, area.width, 2), EditorUtils.HEADER_COLOR);
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
