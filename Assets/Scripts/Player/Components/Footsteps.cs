using UnityEngine;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// This class plays appropriate footstep audio based on the surface the player is walking on. 
    /// It detects terrain using raycasts and selects corresponding audio clips for sand, gravel, 
    /// or general surfaces. The sound is played at set intervals while the player is grounded and 
    /// moving, with slight pitch variation for realism.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Footsteps : MonoBehaviour
    {
        [Header("Footstep Settings")]
        [SerializeField] private AudioSource footstepAudioSource;
        [SerializeField] private LayerMask terrainLayerMask;
        [SerializeField] private float stepInterval = 0.5f;
        [SerializeField, Range(0f, 0.3f)] private float pitchVariation = 0.05f;

        [Header("SFX")]
        [SerializeField] private AudioClip[] footstep_sand;
        [SerializeField] private AudioClip[] footstep_gravel;
        [SerializeField] private AudioClip[] footstep_general;

        private float stepTimer = 0f;

        /// <summary>
        /// Handles the logic for playing footstep sounds while the player is grounded and moving.
        /// Increments a timer based on deltaTime, and plays a sound when the step interval is reached.
        /// </summary>
        /// <param name="currentSpeed">The current speed of the player.</param>
        /// <param name="isGrounded">True if the player is touching the ground, false otherwise.</param>
        public void HandleFootsteps(float currentSpeed, bool isGrounded)
        {
            if (isGrounded && currentSpeed > 0.1f)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepInterval)
                {
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
            else { stepTimer = 0f; }
        }

        /// <summary>
        /// Determines which set of footstep sounds to use by casting a ray downward
        /// and checking the tag of the surface the player is standing on.
        /// </summary>
        /// <returns>An array of <see cref="AudioClip"/> objects appropriate for the surface type.</returns>
        private AudioClip[] DetermineFootstepClips()
        {
            if (
                Physics.Raycast
                (
                    transform.position,
                    Vector3.down,
                    out RaycastHit hit,
                    1.5f, terrainLayerMask
                )
            ) { return GetClipsByTag(hit.collider.tag); }
            return footstep_general;
        }

        /// <summary>
        /// Selects the appropriate array of footstep audio clips based on the surface tag.
        /// Supports tags such as "Sand" and "Gravel", falling back to general sounds if unmatched.
        /// </summary>
        /// <param name="tag">The tag of the surface collider.</param>
        /// <returns>An array of <see cref="AudioClip"/> for the specified surface type.</returns>
        private AudioClip[] GetClipsByTag(string tag)
        {
            return tag switch { "Sand" => footstep_sand, "Gravel" => footstep_gravel, _ => footstep_general };
        }

        /// <summary>
        /// Plays a randomly selected footstep sound from the appropriate clip array.
        /// Adds pitch variation to prevent audio repetition.
        /// </summary>
        private void PlayFootstep()
        {
            AudioClip[] clips = DetermineFootstepClips();
            if (clips.Length == 0) return;

            AudioClip selectedClip = clips[Random.Range(0, clips.Length)];
            footstepAudioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            footstepAudioSource.PlayOneShot(selectedClip);
        }
    }
}
