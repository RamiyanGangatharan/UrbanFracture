using UnityEngine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Handles jumping logic including single and optional double jump mechanics,
    /// as well as detecting landing states.
    /// </summary>
    public class JumpHandler
    {
        private readonly CharacterController controller;
        private readonly CrouchHandler crouchHandler;

        /// <summary>
        /// The constructor initializes the JumpHandler with a reference to the CharacterController and CrouchHandler.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="crouchHandler"></param>
        public JumpHandler(CharacterController controller, CrouchHandler crouchHandler)
        {
            this.controller = controller;
            this.crouchHandler = crouchHandler;
        }

        private float jumpHeight = 2f;
        private float gravityScale = 3f;
        private int timesJumped = 0;
        private bool canDoubleJump = true;
        private bool wasGrounded = false;

        /// <summary>
        /// Initializes the jump handler with the specified character controller.
        /// </summary>
        /// <param name="controller">The CharacterController component associated with the player.</param>
        public JumpHandler(CharacterController controller) { this.controller = controller; }

        /// <summary>
        /// Updates the jump state by tracking grounded status and resetting jump count when grounded.
        /// </summary>
        /// <param name="verticalVelocity">The player's current vertical velocity (passed by reference).</param>
        public void Update(ref float verticalVelocity)
        {
            if (!wasGrounded && controller.isGrounded) { timesJumped = 0; }
            wasGrounded = controller.isGrounded;
        }

        /// <summary>
        /// Attempts to apply a jump by modifying the vertical velocity based on current jump state.
        /// Supports single or double jumps depending on configuration.
        /// </summary>
        /// <param name="verticalVelocity">The player's vertical velocity to modify (passed by reference).</param>
        public void TryJump(ref float verticalVelocity)
        {
            if (crouchHandler.IsCrouching) { return; }

            bool groundedJump = controller.isGrounded && verticalVelocity <= 0f;
            bool doubleJump = canDoubleJump && timesJumped < 2;
            bool singleJump = !canDoubleJump && timesJumped < 1;

            if (!(groundedJump || doubleJump || singleJump)) { return; }

            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
            timesJumped++;
        }

        /// <summary>
        /// Checks if the player has just landed (was airborne and is now grounded).
        /// </summary>
        /// <returns>True if the player has just landed, otherwise false.</returns>
        public bool CheckLanding()
        {
            bool justLanded = !wasGrounded && controller.isGrounded;
            wasGrounded = controller.isGrounded;
            return justLanded;
        }

        /// <summary>
        /// Calculates and returns the gravitational effect on vertical velocity.
        /// </summary>
        public float VerticalVelocity => Mathf.Clamp(Physics.gravity.y * gravityScale, -100f, 100f);
    }
}
