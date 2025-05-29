using System.Collections.Generic;
using UnityEngine;
using UrbanFracture.Core.Player;

namespace UrbanFracture.Player.AI
{
    /// <summary>
    /// This class manages all active enemies in the game.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;

        private List<EnemyController> activeEnemies = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// This function registers an enemy to the manager.
        /// </summary>
        /// <param name="enemy"></param>
        public void RegisterEnemy(EnemyController enemy)
        {
            if (!activeEnemies.Contains(enemy)) { activeEnemies.Add(enemy); }
        }

        /// <summary>
        /// This function unregisters an enemy from the manager.
        /// </summary>
        /// <param name="enemy"></param>
        public void UnregisterEnemy(EnemyController enemy)
        {
            if (activeEnemies.Contains(enemy)) { activeEnemies.Remove(enemy); }
        }

        public IReadOnlyList<EnemyController> ActiveEnemies => activeEnemies.AsReadOnly();
    }
}
