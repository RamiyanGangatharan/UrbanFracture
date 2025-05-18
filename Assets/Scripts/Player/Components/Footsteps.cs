using UnityEngine;

namespace UrbanFracture.Player.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class Footsteps : MonoBehaviour
    {
        [Header("Footstep Settings")]
        [SerializeField] private AudioSource footstepAudioSource;
        [SerializeField] private LayerMask terrainLayerMask;

        [Tooltip("Step interval (seconds) when walking.")]
        [SerializeField] private float walkStepInterval = 0.5f;

        [Tooltip("Step interval (seconds) when sprinting.")]
        [SerializeField] private float sprintStepInterval = 0.3f;

        [Tooltip("Speed above which the player is considered sprinting.")]
        [SerializeField] private float sprintThreshold = 4.5f;

        [SerializeField, Range(0f, 0.3f)] private float pitchVariation = 0.05f;

        [Header("SFX")]
        [SerializeField] private AudioClip[] footstep_sand;
        [SerializeField] private AudioClip[] footstep_gravel;
        [SerializeField] private AudioClip[] footstep_general;

        private float stepTimer = 0f;

        /// <summary>
        /// Handles the logic for playing footstep sounds while the player is grounded and moving.
        /// Adjusts step intervals based on whether the player is walking or sprinting.
        /// </summary>
        /// <param name="currentSpeed">The current speed of the player.</param>
        /// <param name="isGrounded">True if the player is touching the ground.</param>
        public void HandleFootsteps(float currentSpeed, bool isGrounded)
        {
            if (isGrounded && currentSpeed > 0.1f)
            {
                float currentStepInterval = GetStepInterval(currentSpeed);
                stepTimer += Time.deltaTime;

                if (stepTimer >= currentStepInterval)
                {
                    PlayFootstep(currentSpeed);
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = 0f;
            }
        }

        private float GetStepInterval(float speed)
        {
            float minSpeed = 0.1f;
            float maxSpeed = 6f;
            float minInterval = 0.25f;
            float maxInterval = 0.6f;

            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            return Mathf.Lerp(maxInterval, minInterval, (speed - minSpeed) / (maxSpeed - minSpeed));
        }

        private AudioClip[] DetermineFootstepClips()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, terrainLayerMask))
            {
                return GetClipsByTag(hit.collider.tag);
            }

            return footstep_general;
        }

        private AudioClip[] GetClipsByTag(string tag)
        {
            return tag switch
            {
                "Sand" => footstep_sand,
                "Gravel" => footstep_gravel,
                _ => footstep_general,
            };
        }

        private void PlayFootstep(float currentSpeed)
        {
            AudioClip[] clips = DetermineFootstepClips();
            if (clips.Length == 0) return;

            AudioClip selectedClip = clips[Random.Range(0, clips.Length)];

            float basePitch = currentSpeed > sprintThreshold ? 1.1f : 1.0f;
            footstepAudioSource.pitch = basePitch + Random.Range(-pitchVariation, pitchVariation);

            float volume = currentSpeed > sprintThreshold ? 1.0f : 0.7f;
            footstepAudioSource.PlayOneShot(selectedClip, volume);
        }
    }
}
