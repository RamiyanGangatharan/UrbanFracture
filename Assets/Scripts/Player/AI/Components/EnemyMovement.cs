using UnityEngine;
using UnityEngine.AI;
using UrbanFracture.Core.Player;
using UrbanFracture.Player.Components;

namespace UrbanFracture.Player.AI
{
    /// <summary>
    /// This class is responsible for enemy movement towards the player.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(Footsteps))]
    public class EnemyMovement : BaseAI
    {
        private NavMeshAgent agent;
        private EnemyPerception perception;
        private Footsteps footsteps;
        private CharacterController controller;

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            footsteps = GetComponent<Footsteps>();
            controller = GetComponent<CharacterController>();
            agent.speed = 8f;
        }

        protected override void Initialize()
        {
            perception = GetComponent<EnemyPerception>();
        }

        /// <summary>
        /// This function is called every frame to update the enemy's movement towards the player.
        /// </summary>
        protected override void Tick()
        {
            if (perception == null || agent == null || playerTransform == null) return;

            if (!perception.IsPlayerInRange)
            {
                agent.isStopped = true;
                footsteps.HandleFootsteps(0f, false); // Stop footstep audio
                return;
            }

            agent.SetDestination(playerTransform.position);
            agent.isStopped = perception.IsPlayerClose;

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Play footstep audio based on movement and grounded state
            float speed = agent.velocity.magnitude;
            bool isGrounded = true;

            footsteps?.HandleFootsteps(speed, isGrounded);
        }

        public float CurrentSpeed => agent.velocity.magnitude;
    }
}
