using UnityEngine;
using Unity.Cinemachine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Handles dynamic field of view (FOV) changes for the camera, 
    /// typically transitioning between normal and sprinting FOV.
    /// </summary>
    public class CameraFOVHandler
    {
        private readonly CinemachineCamera cam;
        private float normalFOV = 60f;
        private float sprintFOV = 80f;
        private float smoothing = 1f;

        /// <summary>
        /// Initializes the FOV handler with the specified Cinemachine camera.
        /// </summary>
        /// <param name="cam">The Cinemachine camera to control.</param>
        public CameraFOVHandler(CinemachineCamera cam) { this.cam = cam; }

        /// <summary>
        /// Updates the camera’s field of view based on player speed and sprinting state.
        /// </summary>
        /// <param name="speed">The current movement speed of the player.</param>
        /// <param name="sprinting">Whether the player is currently sprinting.</param>
        public void Update(float speed, bool sprinting)
        {
            float targetFOV = sprinting ? Mathf.Lerp(normalFOV, sprintFOV, speed / 15f) : normalFOV;
            cam.Lens.FieldOfView = Mathf.Lerp(cam.Lens.FieldOfView, targetFOV, smoothing * Time.deltaTime);
        }
    }
}
