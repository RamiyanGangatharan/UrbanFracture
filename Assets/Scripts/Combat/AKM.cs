using UnityEngine;
using UnityEngine.InputSystem;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Handles shooting logic specific to an AKM.
    /// Plays muzzle flash and hit effect particles, performs raycast-based hit detection,
    /// and applies damage to objects implementing IDamageable.
    /// </summary>
    public class AKM : Gun
    {
        [Header("Effects")]
        public Transform muzzleFlashSpawnPoint;
        public LayerMask environmentMask;
        public ParticleSystem muzzleFlash;
        public ParticleSystem concreteHitEffectPrefab;
        public ParticleSystem enemyHitEffectPrefab;
        public ParticleSystem gunSmoke;

        [Header("Recoil System")]
        [SerializeField] private RecoilHandler recoilHandler;

        [Header("Fire Mode Configuration")]
        [SerializeField] private float nextTimeToFire = 0f;


        /// <summary>
        /// Updates the pistol each frame by invoking the base gun update.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (gunData == null) return;

            if (gunData.IsAutomatic)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    TryShoot();
                }
            }
            else
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    TryShoot();
                }
            }
        }

        private void FixedUpdate() { recoilHandler?.Tick(Time.fixedDeltaTime); }


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
            PlayGunSmoke();

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
                recoilHandler?.ApplyRecoil();

                Debug.Log($"{gunData.WeaponName} hit {hit.collider.name}");

                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

                if (damageable != null)
                {
                    // Spawn enemy blood hit effect
                    if (enemyHitEffectPrefab != null)
                    {
                        ParticleSystem enemyEffect = Instantiate(
                            enemyHitEffectPrefab,
                            hit.point,
                            Quaternion.LookRotation(hit.normal)
                        );
                        Destroy(enemyEffect.gameObject, 2f);
                    }

                    damageable.TakeDamage(gunData.Damage);
                }
                else
                {
                    // Spawn regular concrete hit effect
                    if (concreteHitEffectPrefab != null)
                    {
                        ParticleSystem concreteEffect = Instantiate(
                            concreteHitEffectPrefab,
                            hit.point,
                            Quaternion.LookRotation(hit.normal)
                        );
                        Destroy(concreteEffect.gameObject, 2f);
                    }
                }
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

        private void PlayGunSmoke()
        {
            if (gunSmoke != null)
            {
                gunSmoke.transform.position = muzzleFlashSpawnPoint.position;
                gunSmoke.transform.rotation = muzzleFlashSpawnPoint.rotation;
                gunSmoke.Play();
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