using UnityEngine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Handles horizontal and vertical character movement, including walking, sprinting,
    /// acceleration, and gravity application using a CharacterController.
    /// </summary>
    public class MovementHandler
    {
        private readonly CharacterController controller;
        private float walkSpeed = 3f;
        private float sprintSpeed = 6f;
        private float acceleration = 10f;
        private float gravityScale = 3f;

        public float verticalVelocity;

        public Vector3 CurrentVelocity { get; private set; }
        public float CurrentSpeed { get; private set; }

        /// <summary>
        /// Initializes the movement handler with a reference to the CharacterController.
        /// </summary>
        /// <param name="controller">The CharacterController component used to move the player.</param>
        public MovementHandler(CharacterController controller) { this.controller = controller; }

        /// <summary>
        /// Updates the player's movement velocity based on input and sprinting state.
        /// Also applies gravity to vertical velocity when not grounded.
        /// </summary>
        /// <param name="moveInput">Directional input for movement.</param>
        /// <param name="isSprinting">Whether the player is sprinting.</param>
        public void Update(Vector2 moveInput, bool isSprinting)
        {
            Vector3 direction = controller.transform.forward * moveInput.y + controller.transform.right * moveInput.x;
            direction.Normalize();

            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            Vector3 targetVelocity = direction * targetSpeed;

            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, targetVelocity, acceleration * Time.deltaTime);

            if (controller.isGrounded && verticalVelocity < 0f) { verticalVelocity = -3f; }
            else { verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime; }

            CurrentSpeed = CurrentVelocity.magnitude;
        }

        /// <summary>
        /// Applies the calculated velocity (including vertical) to the CharacterController.
        /// </summary>
        /// <param name="vertical">The vertical component of the velocity (e.g., from jumping or gravity).</param>
        public void ApplyMovement(float vertical)
        {
            Vector3 fullVelocity = new Vector3(CurrentVelocity.x, vertical, CurrentVelocity.z);
            controller.Move(fullVelocity * Time.deltaTime);
        }
    }
}
