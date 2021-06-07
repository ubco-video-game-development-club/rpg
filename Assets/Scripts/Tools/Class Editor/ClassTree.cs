using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [Serializable]
    public class LayerDictionary : SortedDictionary<int, ClassTier>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<int> levels = new List<int>();
        [SerializeField] private List<ClassTier> tiers = new List<ClassTier>();

        public void OnBeforeSerialize()
        {
            // Store dictionary data in lists
            levels.Clear();
            tiers.Clear();
            foreach (KeyValuePair<int, ClassTier> layer in this)
            {
                levels.Add(layer.Key);
                tiers.Add(layer.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            // Pull dictionary data from lists
            Clear();
            Debug.Log("uwu im gonna take dis now hehe");
            for (int i = 0; i < levels.Count; i++)
            {
                this.Add(levels[i], tiers[i]);
                Debug.Log($"i took {levels[i]}, {tiers[i]}");
            }
        }
    }

    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        public static float TIER_HEIGHT = 54;
        public static float TIER_SPACING = 30;
        public static float MARGIN_WIDTH = 140;
        public static float NODE_WIDTH = 80;
        public static float NODE_HEIGHT = 50;

        [NonSerialized] public ClassTier selectedTier = null;
        [NonSerialized] public ClassNode selectedNode = null;
        [NonSerialized] public int selectedLevel = -1;
        [NonSerialized] public int selectedNodeIndex = -1;

        [SerializeField] private LayerDictionary layers = new LayerDictionary();

        private Vector2 scrollPosition;

        public bool ContainsTier(int level)
        {
            return layers.ContainsKey(level);
        }

        public void AddTier(int level)
        {
            AddTier(level, new ClassTier(level));
        }

        public void AddTier(int level, ClassTier tier)
        {
            Debug.Log($"Apparentlyyyy my mom said we're adding to level {level} apparentlyy");
            layers.Add(level, tier);
            Debug.Log($"dis is wut i did owo");
            foreach (KeyValuePair<int, ClassTier> layer in layers)
            {
                int myLevel = layer.Key;
                ClassTier myTier = layer.Value;
                Debug.Log($"Key: {myLevel}, Tier Data: {myTier.level}");
            }
        }

        public void RemoveTier(int level)
        {
            bool succ = layers.Remove(level);
            Debug.Log($"Succ = {succ}");
        }

        public void MoveTier(int oldLevel, int newLevel)
        {
            Debug.Log($"Moving from {oldLevel} to {newLevel}.");
            ClassTier tier = layers[oldLevel];
            tier.level = newLevel;
            RemoveTier(oldLevel);
            AddTier(newLevel, tier);
        }

        public void AddNode(int level, ClassNodeType nodeType)
        {
            layers[level].AddNode(new ClassNode(nodeType));
        }

        public void RemoveNode(int level, ClassNode node)
        {
            layers[level].RemoveNode(node);
        }

        public void Print()
        {
            Debug.Log("Here's what we got:");
            foreach (KeyValuePair<int, ClassTier> layer in layers)
            {
                int myLevel = layer.Key;
                ClassTier tier = layer.Value;
                Debug.Log($"Key: {myLevel}, Tier Data: {tier.level}");
            }
        }

        public void Draw(Rect area)
        {
            ProcessEvents(Event.current);

            // Create scroll view
            Rect scrollPosRect = new Rect(area.x, area.y, area.width, area.height);
            Rect scrollViewRect = new Rect(area.x, area.y, area.width - 15, layers.Count * TIER_HEIGHT + 10);
            scrollPosition = GUI.BeginScrollView(scrollPosRect, scrollPosition, scrollViewRect);

            int idx = 0;
            foreach (ClassTier tier in layers.Values)
            {
                float yOffset = TIER_SPACING + idx * (TIER_HEIGHT + TIER_SPACING);
                tier.Draw(new Rect(area.x, area.y + yOffset, area.width, TIER_HEIGHT));
                idx++;
            }

            GUI.EndScrollView();

            // Draw vertical divider
            EditorUtils.DrawBox(new Rect(MARGIN_WIDTH + 1, area.y, 3, area.height), EditorUtils.BORDER_COLOR);
        }

        public ClassTier GetTierAt(Vector2 position)
        {
            foreach (KeyValuePair<int, ClassTier> layer in layers)
            {
                ClassTier tier = layer.Value;
                if (tier.Contains(position)) return tier;
            }
            return null;
        }

        public ClassNode GetNodeAt(Vector2 position)
        {
            foreach (KeyValuePair<int, ClassTier> layer in layers)
            {
                ClassTier tier = layer.Value;
                foreach (ClassNode node in tier.nodes)
                {
                    if (node.Contains(position)) return node;
                }
            }
            return null;
        }

        public int IndexOfLevel(int level)
        {
            int idx = 0;
            foreach (int tierLevel in layers.Keys)
            {
                if (tierLevel == level) return idx;
                idx++;
            }
            Debug.Log("Bad news chief.");
            Print();
            Debug.Log("Here's what we want:");
            Debug.Log(level);
            return -1;
        }

        public int IndexOfNode(ClassNode node)
        {
            foreach (ClassTier tier in layers.Values)
            {
                int idx = tier.nodes.IndexOf(node);
                if (idx >= 0) return idx;
            }
            return -1;
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    ClearSelection();

                    ClassTier targetTier = GetTierAt(e.mousePosition);
                    if (targetTier != null)
                    {
                        selectedLevel = targetTier.level;
                    }

                    ClassNode targetNode = GetNodeAt(e.mousePosition);
                    if (targetNode != null)
                    {
                        selectedNode = targetNode;
                        selectedNode.isSelected = true;
                        selectedNodeIndex = IndexOfNode(selectedNode);
                    }
                    else if (targetTier != null)
                    {
                        selectedTier = targetTier;
                        selectedTier.isSelected = true;
                    }
                    e.Use();
                }
            }
        }

        private void ClearSelection()
        {
            if (selectedNode != null) selectedNode.isSelected = false;
            selectedNode = null;
            selectedNodeIndex = -1;

            if (selectedTier != null) selectedTier.isSelected = false;
            selectedTier = null;
            selectedLevel = -1;
        }
    }
}
