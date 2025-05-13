using UnityEngine;

namespace UrbanFracture.Player.Components
{
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
            else
            {
                stepTimer = 0f;
            }
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
                _ => footstep_general
            };
        }

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
