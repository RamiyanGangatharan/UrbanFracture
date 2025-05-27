using UnityEngine;

namespace UrbanFracture.Player.AI
{
    /// <summary>
    /// This class is responsible for detecting the player's presence and distance.
    /// </summary>
    public class EnemyPerception : BaseAI
    {
        public float chaseRange = 10f;
        public float stopDistance = 2f;

        /// <summary>
        /// This property holds the distance to the player.
        /// </summary>
        public float DistanceToPlayer { get; private set; }

        /// <summary>
        /// This property checks if the player is within the chase range.
        /// </summary>
        public bool IsPlayerInRange => DistanceToPlayer <= chaseRange;

        /// <summary>
        /// This property checks if the player is close enough to stop chasing.
        /// </summary>
        public bool IsPlayerClose => DistanceToPlayer <= stopDistance;

        protected override void Initialize() { }

        /// <summary>
        /// This function is called every frame to update the enemy's perception of the player.
        /// </summary>
        protected override void Tick()
        {
            DistanceToPlayer = Vector3.Distance(
                transform.position, 
                playerTransform.position
            );
        }
    }
}
