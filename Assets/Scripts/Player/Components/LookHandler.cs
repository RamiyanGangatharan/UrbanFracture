using UnityEngine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Handles camera and player body rotation based on look input,
    /// including vertical pitch clamping and sensitivity scaling.
    /// </summary>
    public class LookHandler
    {
        private readonly Transform body;
        private readonly Transform cameraTransform;
        private Vector2 sensitivity = new(0.1f, 0.1f);
        private float pitch = 0f;
        private float pitchLimit = 40f;

        /// <summary>
        /// Initializes the look handler with player body and camera references.
        /// </summary>
        /// <param name="body">The transform of the player’s body (typically the root object).</param>
        /// <param name="cameraTransform">The transform of the camera or camera follow target.</param>
        public LookHandler(Transform body, Transform cameraTransform)
        {
            this.body = body;
            this.cameraTransform = cameraTransform;
        }

        /// <summary>
        /// Updates the camera and body rotation based on mouse or analog stick input.
        /// Applies pitch clamping to limit vertical view angle.
        /// </summary>
        /// <param name="lookInput">The 2D input vector representing horizontal and vertical look direction.</param>
        public void Update(Vector2 lookInput)
        {
            // Update vertical look (pitch)
            pitch -= lookInput.y * sensitivity.y;
            pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

            // Apply pitch locally to the camera transform (up/down)
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            // Rotate the player body horizontally (yaw)
            body.Rotate(Vector3.up * lookInput.x * sensitivity.x);
        }

        /// <summary>
        /// Optionally set sensitivity for look input
        /// </summary>
        public void SetSensitivity(float horizontal, float vertical)
        {
            sensitivity = new Vector2(horizontal, vertical);
        }

        /// <summary>
        /// Optionally set vertical look limits
        /// </summary>
        public void SetPitchLimit(float limit)
        {
            pitchLimit = limit;
        }
    }
}
