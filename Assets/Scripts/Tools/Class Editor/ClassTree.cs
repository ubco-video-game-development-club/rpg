using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG;

namespace ClassEditor
{
    [Serializable]
    public class LayerDictionary : SortedList<int, ClassTier>, ISerializationCallbackReceiver
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
            for (int i = 0; i < levels.Count; i++)
            {
                this.Add(levels[i], tiers[i]);
            }
        }
    }

    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        public static float TIER_HEIGHT = 54;
        public static float TIER_SPACING = 30;
        public static float LEVEL_MARGIN_WIDTH = 100;
        public static float TYPE_MARGIN_WIDTH = 80;
        public static float NODE_WIDTH = 80;
        public static float NODE_HEIGHT = 50;

        [NonSerialized] public ClassTier selectedTier = null;
        [NonSerialized] public ClassNode selectedNode = null;
        [NonSerialized] public int selectedLevel = -1;
        [NonSerialized] public int selectedNodeIndex = -1;

        [SerializeField] private LayerDictionary layers = new LayerDictionary();
        public LayerDictionary Layers { get => layers; }

        public bool IsEditingPath { get => pathStartNode != null; }

        private ClassNode pathStartNode = null;
        private Vector2 scrollPosition;

        public bool ContainsTier(int level)
        {
            return layers.ContainsKey(level);
        }

        public ClassTierType GetTierType(int level)
        {
            return layers[level].tierType;
        }

        public LevelUpOption[] GetSkillOptions(int level)
        {
            return layers[level].nodes[0].levelUpOptions;
        }

        public ClassBaseStats GetClassBaseStats()
        {
            return layers[1].nodes[0].classBaseStats;
        }

        public List<ClassNode> GetChildren(ClassNode parent)
        {
            List<ClassNode> children = new List<ClassNode>();
            int childTierIdx = IndexOfLevel(parent.level) + 1;
            if (layers.Count > childTierIdx)
            {
                int childLevel = layers.Values[childTierIdx].level;
                List<ClassNode> childNodes = layers[childLevel].nodes;
                foreach (int childIdx in parent.childIndices)
                {
                    children.Add(childNodes[childIdx]);
                }
            }
            return children;
        }

        public int IndexOfLevel(int level)
        {
            return layers.IndexOfKey(level);
        }

        public int IndexOfNode(ClassNode node)
        {
            return layers[node.level].nodes.IndexOf(node);
        }

#if UNITY_EDITOR
        public void Initialize()
        {
            ClearSelection();
            pathStartNode = null;
        }

        public void AddTier(int level, ClassTierType type)
        {
            AddTier(level, new ClassTier(level, type));
        }

        public void AddTier(int level, ClassTier tier)
        {
            ClearSelection();
            layers.Add(level, tier);
            int parentTierIdx = IndexOfLevel(level) - 1;
            if (parentTierIdx >= 0)
            {
                foreach (ClassNode parent in layers.Values[parentTierIdx].nodes)
                {
                    parent.childIndices.Clear();
                }
            }
        }

        public void RemoveTier(int level)
        {
            ClearSelection();
            int parentTierIdx = IndexOfLevel(level) - 1;
            if (parentTierIdx >= 0)
            {
                foreach (ClassNode parent in layers.Values[parentTierIdx].nodes)
                {
                    parent.childIndices.Clear();
                }
            }
            layers.Remove(level);
        }

        public void MoveTier(int oldLevel, int newLevel)
        {
            ClearSelection();
            ClassTier tier = layers[oldLevel];
            tier.level = newLevel;
            foreach (ClassNode node in tier.nodes)
            {
                node.level = newLevel;
                node.childIndices.Clear();
            }
            RemoveTier(oldLevel);
            AddTier(newLevel, tier);
            EditorUtility.SetDirty(this);
        }

        public void AddNode(int level)
        {
            layers[level].AddNode();
        }

        public void RemoveNode(ClassNode node)
        {
            ClearSelection();
            int nodeIdx = IndexOfNode(node);
            int parentTierIdx = IndexOfLevel(node.level) - 1;
            foreach (ClassNode parent in layers.Values[parentTierIdx].nodes)
            {
                if (parent.childIndices.Count <= 0) continue;
                parent.childIndices.Remove(nodeIdx);
                for (int i = 0; i < parent.childIndices.Count; i++)
                {
                    if (parent.childIndices[i] > nodeIdx)
                    {
                        parent.childIndices[i]--;
                    }
                }
            }
            layers[node.level].RemoveNode(node);
        }

        public void Draw(Rect area)
        {
            // Create scroll view
            Rect scrollPosRect = new Rect(area.x, area.y, area.width, area.height);
            Rect scrollViewRect = new Rect(area.x, area.y, area.width - 15, layers.Count * TIER_HEIGHT + 10);
            scrollPosition = GUI.BeginScrollView(scrollPosRect, scrollPosition, scrollViewRect);

            // Draw tiers
            int idx = 0;
            foreach (ClassTier tier in layers.Values)
            {
                float yOffset = TIER_SPACING + idx * (TIER_HEIGHT + TIER_SPACING);
                tier.Draw(this, new Rect(area.x, area.y + yOffset, area.width, TIER_HEIGHT));
                idx++;
            }

            // Draw paths
            foreach (ClassTier tier in layers.Values)
            {
                tier.DrawPaths(this);
            }

            // Draw nodes
            idx = 0;
            foreach (ClassTier tier in layers.Values)
            {
                float yOffset = TIER_SPACING + idx * (TIER_HEIGHT + TIER_SPACING);
                tier.DrawNodes(this, new Rect(area.x, area.y + yOffset, area.width, TIER_HEIGHT));
                idx++;
            }

            GUI.EndScrollView();

            // Draw vertical divider for level
            EditorUtils.DrawBox(new Rect(LEVEL_MARGIN_WIDTH + 1, area.y, 3, area.height), EditorUtils.BORDER_COLOR);

            // Draw vertical divider for type
            EditorUtils.DrawBox(new Rect(LEVEL_MARGIN_WIDTH + TYPE_MARGIN_WIDTH + 1, area.y, 3, area.height), EditorUtils.BORDER_COLOR);

            // Draw path edit line
            if (IsEditingPath)
            {
                Vector2 startPos = pathStartNode.ButtonPosition;
                Vector2 mousePos = Event.current.mousePosition;
                Handles.DrawBezier(startPos, mousePos, startPos, mousePos, EditorUtils.LINE_COLOR, null, 3f);
                GUI.changed = true;
            }

            ProcessEvents(Event.current);
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

        public void StartPathEdit(ClassNode startNode)
        {
            pathStartNode = startNode;
        }

        private void FinishPathEdit(ClassNode endNode)
        {
            pathStartNode.AddChild(IndexOfNode(endNode));
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    ClearSelection();

                    if (IsEditingPath)
                    {
                        ClassTier targetTier = GetTierAt(e.mousePosition);
                        if (targetTier != null)
                        {
                            ClassNode targetNode = GetNodeAt(e.mousePosition);
                            if (targetNode != null)
                            {
                                int startLevelIdx = IndexOfLevel(pathStartNode.level);
                                int targetLevelIdx = IndexOfLevel(targetNode.level);
                                if (targetLevelIdx == startLevelIdx + 1) FinishPathEdit(targetNode);
                            }
                        }

                        pathStartNode = null;
                    }
                    else
                    {
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
#endif
    }
}
