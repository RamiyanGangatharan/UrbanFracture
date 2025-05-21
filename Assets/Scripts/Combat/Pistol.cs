using UnityEngine;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Handles shooting logic specific to a pistol.
    /// Plays muzzle flash and hit effect particles, performs raycast-based hit detection,
    /// and applies damage to objects implementing IDamageable.
    /// </summary>
    public class Pistol : Gun
    {
        [Header("Effects")]
        public ParticleSystem muzzleFlash;
        public Transform muzzleFlashSpawnPoint;
        public ParticleSystem hitEffectParticleSystem;

        /// <summary>
        /// Updates the pistol each frame by invoking the base gun update.
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Executes the pistol's shooting behavior:
        /// - Plays muzzle flash
        /// - Raycasts for hit detection
        /// - Spawns hit effects
        /// - Applies damage to IDamageable targets
        /// </summary>
        public override void Shoot()
        {
            PlayMuzzleFlash();

            if (
                Physics.Raycast(
                    cameraTransform.position,
                    cameraTransform.forward,
                    out RaycastHit hit,
                    gunData.Range,
                    gunData.TargetLayerMask
                )
            )
            {
                Debug.Log($"{gunData.WeaponName} hit {hit.collider.name}");

                SpawnHitEffect(hit);
                ApplyDamage(hit);
            }
        }

        /// <summary>
        /// Plays the muzzle flash effect at the muzzle flash spawn point.
        /// </summary>
        private void PlayMuzzleFlash()
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
        }

        /// <summary>
        /// Spawns the hit effect particle system at the point of impact.
        /// </summary>
        /// <param name="hit">Raycast hit information</param>
        private void SpawnHitEffect(RaycastHit hit)
        {
            if (hitEffectParticleSystem != null)
            {
                var hitEffect = Instantiate(
                    hitEffectParticleSystem,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                hitEffect.Play();

                float destroyDelay = hitEffect.main.duration + hitEffect.main.startLifetime.constantMax;
                Destroy(hitEffect.gameObject, destroyDelay);
            }
        }

        /// <summary>
        /// Applies damage to any target that implements the IDamageable interface.
        /// </summary>
        /// <param name="hit">Raycast hit information</param>
        private void ApplyDamage(RaycastHit hit)
        {
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(gunData.Damage);
            }
        }
    }
}
