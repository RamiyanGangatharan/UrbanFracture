using Unity.Cinemachine;
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
        private readonly CinemachineCamera camera;
        private Vector2 sensitivity = new(0.1f, 0.1f);
        private float pitch = 0f;
        private float pitchLimit = 40f;

        /// <summary>
        /// Initializes the look handler with player body and camera references.
        /// </summary>
        /// <param name="body">The transform of the player’s body (typically the root object).</param>
        /// <param name="cam">The Cinemachine camera used for first-person view.</param>
        public LookHandler(Transform body, CinemachineCamera cam)
        {
            this.body = body;
            this.camera = cam;
        }

        /// <summary>
        /// Updates the camera and body rotation based on mouse or analog stick input.
        /// Applies pitch clamping to limit vertical view angle.
        /// </summary>
        /// <param name="lookInput">The 2D input vector representing horizontal and vertical look direction.</param>
        public void Update(Vector2 lookInput)
        {
            pitch -= lookInput.y * sensitivity.y;
            pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

            camera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            body.Rotate(Vector3.up * lookInput.x * sensitivity.x);
        }
    }
}
