using Unity.Cinemachine;
using UnityEngine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// Adds a camera bobbing effect using Cinemachine noise based on player movement speed.
    /// </summary>
    public class CameraBob : MonoBehaviour
    {
        [Header("Camera Bob Settings")]
        [SerializeField] private float bobFrequency = 1f;
        [SerializeField] private float bobAmplitude = 0.5f;

        [Header("References")]
        [SerializeField] private CinemachineCamera cameraReference;

        private CinemachineBasicMultiChannelPerlin noise;
        private float bobTimer;

        /// <summary>
        /// Initializes the noise component from the Cinemachine camera reference.
        /// </summary>
        private void Awake()
        {
            if (cameraReference != null)
            {
                cameraReference.TryGetComponent(out noise);
            }
        }

        /// <summary>
        /// Updates the camera bob effect based on movement speed and grounded status.
        /// </summary>
        /// <param name="speed">Current movement speed of the player.</param>
        /// <param name="isGrounded">Whether the player is on the ground.</param>
        public void UpdateCameraBob(float speed, bool isGrounded)
        {
            if (noise == null) return;

            if (isGrounded && speed > 0.1f)
            {
                bobTimer += Time.deltaTime * bobFrequency;
                float bobAmount = Mathf.Sin(bobTimer) * bobAmplitude;

                noise.AmplitudeGain = Mathf.Abs(bobAmount);
                noise.FrequencyGain = bobFrequency;
            }
            else
            {
                // Smoothly reduce the bobbing when idle or in air
                noise.AmplitudeGain = Mathf.Lerp(noise.AmplitudeGain, 0f, Time.deltaTime * 5f);
                noise.FrequencyGain = Mathf.Lerp(noise.FrequencyGain, 0f, Time.deltaTime * 5f);
                bobTimer = 0f;
            }
        }
    }
}
