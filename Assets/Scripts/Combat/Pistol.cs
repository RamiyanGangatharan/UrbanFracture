using UnityEngine;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Handles shooting logic specific to a pistol.
    /// Plays muzzle flash and hit effect particles, and performs raycast-based hit detection.
    /// Inherits core behavior from the Gun base class.
    /// </summary>
    public class Pistol : Gun
    {
        [Header("Effects")]
        public ParticleSystem muzzleFlash;
        public Transform muzzleFlashSpawnPoint;
        public ParticleSystem hitEffectParticleSystem;

        /// <summary>
        /// Updates the pistol each frame by invoking the base gun update (e.g., recoil reset).
        /// </summary>
        public override void Update() { base.Update(); }

        /// <summary>
        /// Executes the pistol's shooting behavior by playing a muzzle flash at the designated 
        /// spawn point, performing a ray cast to detect any hit targets, and spawning a particle 
        /// effect at the impact location.
        /// </summary>
        public override void Shoot()
        {
            if (muzzleFlash != null)
            {
                if (muzzleFlashSpawnPoint != null)
                {
                    muzzleFlash.transform.position = muzzleFlashSpawnPoint.position;
                    muzzleFlash.transform.rotation = muzzleFlashSpawnPoint.rotation;
                }
                muzzleFlash.Play();
            }

            if (
                Physics.Raycast
                (
                    cameraTransform.position,
                    cameraTransform.forward,
                    out RaycastHit hit,
                    gunData.Range,
                    gunData.TargetLayerMask
                )
            )
            {
                Debug.Log($"{gunData.WeaponName} hit {hit.collider.name}");

                if (hitEffectParticleSystem != null)
                {
                    var hitEffect = Instantiate
                    (
                        hitEffectParticleSystem,
                        hit.point,
                        Quaternion.LookRotation(hit.normal)
                    );

                    hitEffect.Play();

                    float destroyDelay = hitEffect.main.duration + hitEffect.main.startLifetime.constantMax;
                    Destroy(hitEffect.gameObject, destroyDelay);
                }
            }
        }
    }
}
