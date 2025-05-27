using UnityEngine;

namespace UrbanFracture.Player.AI
{
    /// <summary>
    /// This class handles enemy animations based on movement speed and sprinting state.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : BaseAI
    {
        public float sprintThreshold = 5f;

        private Animator animator;
        private EnemyMovement movement;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void Initialize()
        {
            movement = GetComponent<EnemyMovement>();
            if (movement == null) { Debug.LogError("EnemyMovement component missing!"); }
        }

        /// <summary>
        /// This function is called every frame to update the enemy's animation state.
        /// </summary>
        protected override void Tick()
        {
            float speed = movement.CurrentSpeed;
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsSprinting", speed > sprintThreshold);
        }
    }
}
