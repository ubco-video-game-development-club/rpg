using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        private SortedDictionary<int, List<ClassTreeNode>> levelUpTiers = new SortedDictionary<int, List<ClassTreeNode>>();
        public SortedDictionary<int, List<ClassTreeNode>> LevelUpTiers { get => levelUpTiers; }

        public bool ContainsTier(int level)
        {
            return levelUpTiers.ContainsKey(level);
        }

        public void AddTier(int level)
        {
            levelUpTiers.Add(level, new List<ClassTreeNode>());
        }

        public void RemoveTier(int level)
        {
            levelUpTiers.Remove(level);
        }

        public void AddNode(int level, ClassTreeNode node)
        {
            if (levelUpTiers.ContainsKey(level))
            {
                levelUpTiers[level].Add(node);
            }
            else
            {
                Debug.LogWarning("Attempted to add a node to a level that does not exist!");
            }
        }

        public void RemoveNode(int level, ClassTreeNode node)
        {
            if (levelUpTiers.ContainsKey(level))
            {
                levelUpTiers[level].Remove(node);
            }
            else
            {
                Debug.LogWarning("Attempted to remove a node from a level that does not exist!");
            }
        }
    }
}
