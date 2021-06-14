using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    public enum ClassTierType
    {
        Class, Subclass, Skill
    }

    [System.Serializable]
    public class ClassTier
    {
        public int level;
        public ClassTierType tierType;
        public List<ClassNode> nodes;

        [NonSerialized] public bool isSelected;
        private Rect displayRect;

        public ClassTier(int level, ClassTierType tierType)
        {
            this.level = level;
            this.tierType = tierType;
            nodes = new List<ClassNode>();
            if (tierType == ClassTierType.Skill)
            {
                nodes.Add(new ClassNode(level, ClassTierType.Skill));
            }
        }

        public void AddNode()
        {
            nodes.Add(new ClassNode(level, tierType));
        }

        public void RemoveNode(ClassNode node)
        {
            nodes.Remove(node);
        }

        public void Draw(ClassTree tree, Rect area)
        {
            displayRect = area;
            float levelMarginWidth = ClassTree.LEVEL_MARGIN_WIDTH;
            float typeMarginWidth = ClassTree.TYPE_MARGIN_WIDTH;

            Color dividerColor = isSelected ? EditorUtils.HIGHLIGHTED_COLOR : EditorUtils.BACKGROUND_COLOR;

            // Draw dividing lines
            EditorUtils.DrawBox(new Rect(area.x, area.y, area.width, 2), dividerColor);
            EditorUtils.DrawBox(new Rect(area.x, area.y + area.height, area.width, 2), dividerColor);

            // Draw margin labels
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            string levelText = EditorUtils.TrimStringToFit($"Level {level}", levelMarginWidth, labelStyle);
            GUI.Label(new Rect(area.x + 1, area.y + area.height / 2 - 10, levelMarginWidth, 20), levelText, labelStyle);
            string typeText = EditorUtils.TrimStringToFit($"{tierType.ToString()}", typeMarginWidth, labelStyle);
            GUI.Label(new Rect(area.x + levelMarginWidth + 3, area.y + area.height / 2 - 10, typeMarginWidth, 20), typeText, labelStyle);
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
            float levelMarginWidth = ClassTree.LEVEL_MARGIN_WIDTH;
            float typeMarginWidth = ClassTree.TYPE_MARGIN_WIDTH;
            float nodeWidth = ClassTree.NODE_WIDTH;
            float nodeHeight = ClassTree.NODE_HEIGHT;

            float marginWidth = levelMarginWidth + typeMarginWidth;

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
