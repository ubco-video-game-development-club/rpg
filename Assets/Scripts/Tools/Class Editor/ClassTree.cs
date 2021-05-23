using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassEditor
{
    [CreateAssetMenu(fileName = "new ClassTree", menuName = "Class Tree", order = 67)]
    public class ClassTree : ScriptableObject
    {
        private Dictionary<int, List<ClassTreeNode>> levelUpTiers = new Dictionary<int, List<ClassTreeNode>>();
        public Dictionary<int, List<ClassTreeNode>> LevelUpTiers { get => levelUpTiers; }

        public void AddTier(int level)
        {
            levelUpTiers[level] = new List<ClassTreeNode>();
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
