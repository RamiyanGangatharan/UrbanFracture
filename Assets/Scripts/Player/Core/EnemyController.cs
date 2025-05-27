using UnityEngine;
using UrbanFracture.Player.AI;

namespace UrbanFracture.Core.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyController : BaseCharacterController
    {
        public void DebugTakeDamage(float amount)
        {
            TakeDamage(amount);
        }

        private void OnEnable()
        {
            EnemyManager.Instance?.RegisterEnemy(this);
        }

        private void OnDisable()
        {
            EnemyManager.Instance?.UnregisterEnemy(this);
        }
    }
}
