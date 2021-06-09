using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [System.Serializable]
    public class ClassTier
    {
        public int level;
        public List<ClassNode> nodes;

        [NonSerialized] public bool isSelected;
        private Rect displayRect;

        public ClassTier(int level)
        {
            this.level = level;
            nodes = new List<ClassNode>();
        }

        public void AddNode(ClassNode node)
        {
            nodes.Add(node);
        }

        public void RemoveNode(ClassNode node)
        {
            nodes.Remove(node);
        }

        public void Draw(ClassTree tree, Rect area)
        {
            displayRect = area;
            float marginWidth = ClassTree.MARGIN_WIDTH;

            Color dividerColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BACKGROUND_COLOR;

            // Draw dividing lines
            EditorUtils.DrawBox(new Rect(area.x, area.y, area.width, 2), dividerColor);
            EditorUtils.DrawBox(new Rect(area.x, area.y + area.height, area.width, 2), dividerColor);

            // Draw level label
            GUI.Label(new Rect(area.x + marginWidth - 60, area.y + area.height / 2 - 10, 60, 20), $"Level {level}");
        }

        public void DrawPaths(ClassTree tree)
        {
            foreach (ClassNode node in nodes)
            {
                node.DrawPaths(tree);
            }
        }

        public void DrawNodes(ClassTree tree, Rect area)
        {
            float tierheight = ClassTree.TIER_HEIGHT;
            float marginWidth = ClassTree.MARGIN_WIDTH;
            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;

            // Draw nodes
            int idx = 0;
            float sectionWidth = (area.width - marginWidth) / (nodes.Count + 1);
            float yOffset = (tierheight - nodeHeight) / 2 + 1;
            foreach (ClassNode node in nodes)
            {
                float xOffset = marginWidth + sectionWidth * (idx + 1) - nodeWidth / 2;
                node.Draw(tree, new Vector2(area.x + xOffset, area.y + yOffset));
                idx++;
            }
        }

        public bool Contains(Vector2 position) => displayRect.Contains(position);
    }
}
