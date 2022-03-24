using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class EnemyCondition : Condition
    {
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private int minKillCount = 1;

        private int enemyStartCount;
        private int enemyCount;

        public override void Initialize()
        {
            enemyStartCount = enemies.Count;
            enemyCount = enemies.Count;
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath.AddListener(RemoveEnemy);
            }
        }

        public override bool Evaluate()
        {
            int enemiesKilled = enemyStartCount - enemyCount;
            return enemiesKilled >= minKillCount;
        }

        private void RemoveEnemy()
        {
            enemyCount--;
        }
    }
}
