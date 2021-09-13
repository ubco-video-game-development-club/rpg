using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Actor[] actorPrefabs;
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private float respawnDelay = 10f;

        private YieldInstruction respawnDelayInstruction;

        void Awake()
        {
            respawnDelayInstruction = new WaitForSeconds(respawnDelay);
            for (int i = 0; i < actorPrefabs.Length; i++)
            {
                SpawnActor(i);
            }
        }

        private void SpawnActor(int spawnIdx)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Actor actorInstance = Instantiate(actorPrefabs[spawnIdx], spawnPos, Quaternion.identity);
            actorInstance.OnDeath.AddListener(() => StartCoroutine(RespawnTimer(spawnIdx)));
        }

        private IEnumerator RespawnTimer(int spawnIdx)
        {
            yield return respawnDelayInstruction;
            SpawnActor(spawnIdx);
        }
    }
}
