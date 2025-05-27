using UnityEngine;
using UnityEngine.AI;

namespace UrbanFracture.Player.AI
{
    /// <summary>
    /// This class is responsible for enemy movement towards the player.
    /// </summary>
    public class EnemyMovement : BaseAI
    {
        private NavMeshAgent agent;
        private EnemyPerception perception;

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }

        protected override void Initialize()
        {
            perception = GetComponent<EnemyPerception>();
            if (perception == null) { Debug.LogError("EnemyPerception component missing!"); }
        }

        /// <summary>
        /// This function is called every frame to update the enemy's movement towards the player.
        /// </summary>
        protected override void Tick()
        {
            if (!perception.IsPlayerInRange)
            {
                agent.isStopped = true;
                return;
            }

            agent.SetDestination(playerTransform.position);
            agent.isStopped = perception.IsPlayerClose;

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * 5f
                );
            }
        }

        public float CurrentSpeed => agent.velocity.magnitude;
    }
}
