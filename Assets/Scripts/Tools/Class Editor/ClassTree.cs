using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        private SortedDictionary<int, ClassTreeTier> layers = new SortedDictionary<int, ClassTreeTier>();

        public bool ContainsTier(int level)
        {
            return layers.ContainsKey(level);
        }

        public void AddTier(int level)
        {
            layers.Add(level, new ClassTreeTier(level));
        }

        public void RemoveTier(int level)
        {
            layers.Remove(level);
        }

        public void AddNode(int level, ClassTreeNode node)
        {
            layers[level].AddNode(node);
        }

        public void RemoveNode(int level, ClassTreeNode node)
        {
            layers[level].RemoveNode(node);
        }

        /// Editor Functions

        public static float TIER_HEIGHT = 54;
        public static float TIER_SPACING = 30;
        public static float MARGIN_WIDTH = 140;
        public static float NODE_WIDTH = 80;
        public static float NODE_HEIGHT = 50;

        private Vector2 scrollPosition;

        public void Draw(Rect area)
        {
            // Create scroll view
            Rect scrollPosRect = new Rect(area.x, area.y, area.width, area.height);
            Rect scrollViewRect = new Rect(area.x, area.y, area.width - 15, layers.Count * TIER_HEIGHT + 10);
            scrollPosition = GUI.BeginScrollView(scrollPosRect, scrollPosition, scrollViewRect);

            int idx = 0;
            foreach (ClassTreeTier tier in layers.Values)
            {
                float yOffset = TIER_SPACING + idx * (TIER_HEIGHT + TIER_SPACING);
                tier.Draw(new Rect(area.x, area.y + yOffset, area.width, TIER_HEIGHT));
                idx++;
            }

            GUI.EndScrollView();
        }

        public ClassTreeTier GetTierAt(Vector2 position)
        {
            foreach (KeyValuePair<int, ClassTreeTier> layer in layers)
            {
                ClassTreeTier tier = layer.Value;
                if (tier.Contains(position)) return tier;
            }
            return null;
        }

        public ClassTreeNode GetNodeAt(Vector2 position)
        {
            foreach (KeyValuePair<int, ClassTreeTier> layer in layers)
            {
                ClassTreeTier tier = layer.Value;
                foreach (ClassTreeNode node in tier.Nodes)
                {
                    if (node.Contains(position)) return node;
                }
            }
            return null;
        }
    }
}
