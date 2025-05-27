using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UrbanFracture.Core.Player;
using UrbanFracture.Player.Components;

namespace UrbanFracture.Player.AI
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public List<Transform> spawnPoints;
        public float spawnRadius = 20f;
        public int numberToSpawn = 5;

        private void Start() { SpawnInitialEnemies(); }

        /// <summary>
        /// Spawns the initial set of enemies, distributing them evenly across all spawn points.
        /// </summary>
        private void SpawnInitialEnemies()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Debug.LogWarning("No spawn points assigned!");
                return;
            }

            /*
             This block evenly distributes a specified number of enemies (`numberToSpawn`) 
             across all available spawn points. It first calculates the number of spawn 
             points (`spawnPointsCount`), then determines how many enemies to spawn at 
             each point (`baseSpawnCount`) by dividing the total number by the number of 
             points. Any leftover enemies (the `remainder`) that couldn't be evenly divided 
             are then distributed by adding one extra enemy to the first few spawn points until 
             the remainder is exhausted.

             For each spawn point `i`, it calculates how many enemies should be spawned at 
             that point by adding 1 to `baseSpawnCount` if `i` is less than the remainder. 
             Then, it spawns that many enemies at the given spawn point by calling 
             `SpawnEnemyAtPoint(spawnPoints[i])` in a nested loop.

             This ensures that all enemies are distributed as evenly as possible, with a 
             small random variation to account for any division remainders.
            */
            int spawnPointsCount = spawnPoints.Count;
            int baseSpawnCount = numberToSpawn / spawnPointsCount;
            int remainder = numberToSpawn % spawnPointsCount;

            for (int i = 0; i < spawnPointsCount; i++)
            {
                int spawnCount = baseSpawnCount + (i < remainder ? 1 : 0);
                for (int j = 0; j < spawnCount; j++) { SpawnEnemyAtPoint(spawnPoints[i]); }
            }
        }

        /// <summary>
        /// Spawns a single enemy at a random spawn point.
        /// </summary>
        private void SpawnEnemy()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Debug.LogWarning("No spawn points assigned!");
                return;
            }
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnEnemyAtPoint(randomPoint);
        }

        /// <summary>
        /// Spawns a single enemy at a specified spawn point and registers it with the EnemyManager.
        /// Also attaches a death listener to handle respawning and deregistration.
        /// </summary>
        /// <param name="spawnPoint">The transform to spawn the enemy near.</param>
        private void SpawnEnemyAtPoint(Transform spawnPoint)
        {
            Vector3 spawnPosition = GetRandomNavMeshPosition(spawnPoint);
            GameObject newEnemy = Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity
            );

            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            Health health = newEnemy.GetComponent<Health>();

            if (enemyController != null)
            {
                EnemyManager.Instance.RegisterEnemy(enemyController);
            }

            if (health != null)
            {
                health.OnDeath.AddListener(() =>
                    {
                        if (enemyController != null)
                        {
                            EnemyManager.Instance.UnregisterEnemy(enemyController);
                        }
                        SpawnEnemy();
                    }
                );
            }
            else { Debug.LogWarning("Spawned enemy is missing Health component!"); }
        }

        /// <summary>
        /// Returns a valid random position on the NavMesh within the spawn radius of the given spawn point.
        /// </summary>
        /// <param name="spawnPoint">The reference transform used to determine the spawn area.</param>
        /// <returns>A valid position on the NavMesh near the spawn point, or the spawn point position if none found.</returns>
        private Vector3 GetRandomNavMeshPosition(Transform spawnPoint)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius + spawnPoint.position;
            if
            (
                NavMesh.SamplePosition(
                    randomDirection,
                    out NavMeshHit hit,
                    5f, NavMesh.AllAreas
                )
            )
            { return hit.position; }
            return spawnPoint.position;
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize spawn radii for each assigned spawn point.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (spawnPoints != null)
            {
                Gizmos.color = Color.green;
                foreach (var point in spawnPoints)
                {
                    if (point != null)
                    {
                        Gizmos.DrawWireSphere(
                            point.position,
                            spawnRadius
                        );
                    }
                }
            }
        }
    }
}
