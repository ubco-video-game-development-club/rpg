using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        public static float TIER_HEIGHT = 54;
        public static float TIER_SPACING = 30;
        public static float MARGIN_WIDTH = 140;
        public static float NODE_WIDTH = 80;
        public static float NODE_HEIGHT = 50;

        public ClassTreeTier selectedTier = null;
        public ClassTreeNode selectedNode = null;

        private SortedDictionary<int, ClassTreeTier> layers = new SortedDictionary<int, ClassTreeTier>();
        
        private Vector2 scrollPosition;

        public bool ContainsTier(int level)
        {
            return layers.ContainsKey(level);
        }

        public void AddTier(int level)
        {
            AddTier(level, new ClassTreeTier(level));
        }

        public void AddTier(int level, ClassTreeTier tier)
        {
            layers.Add(level, new ClassTreeTier(level));
        }

        public void RemoveTier(int level)
        {
            layers.Remove(level);
        }

        public void MoveTier(int oldLevel, int newLevel)
        {
            ClassTreeTier tier = layers[oldLevel];
            RemoveTier(oldLevel);
            AddTier(newLevel, tier);
        }

        public void AddNode(int level)
        {
            AddNode(level, new ClassTreeNode());
        }

        public void AddNode(int level, ClassTreeNode node)
        {
            layers[level].AddNode(node);
        }

        public void RemoveNode(int level, ClassTreeNode node)
        {
            layers[level].RemoveNode(node);
        }

        public void Draw(Rect area)
        {
            ProcessEvents(Event.current);

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

            // Draw vertical divider
            EditorUtils.DrawBox(new Rect(MARGIN_WIDTH + 1, area.y, 3, area.height), EditorUtils.BORDER_COLOR);
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
                foreach (ClassTreeNode node in tier.nodes)
                {
                    if (node.Contains(position)) return node;
                }
            }
            return null;
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    // Clear current selection
                    if (selectedNode != null) selectedNode.isSelected = false;
                    selectedNode = null;
                    if (selectedTier != null) selectedTier.isSelected = false;
                    selectedTier = null;

                    ClassTreeNode targetNode = GetNodeAt(e.mousePosition);
                    if (targetNode != null)
                    {
                        selectedNode = targetNode;
                        selectedNode.isSelected = true;
                    }
                    else
                    {
                        ClassTreeTier targetTier = GetTierAt(e.mousePosition);
                        if (targetTier != null)
                        {
                            Debug.Log(targetTier.level);
                            selectedTier = targetTier;
                            selectedTier.isSelected = true;
                        }
                    }
                    e.Use();
                }
            }
        }
    }
}
