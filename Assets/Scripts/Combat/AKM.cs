using UnityEngine;
using UnityEngine.InputSystem;
using UrbanFracture.Player.Components;

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
        public ParticleSystem muzzleFlash;
        public ParticleSystem hitEffectPrefab;
        public ParticleSystem gunSmoke;

        [Header("Recoil System")]
        [SerializeField] public Transform recoilCamera;
        [SerializeField] public Transform weaponTransform;
        [SerializeField] public Vector3 weaponRecoilKick = new Vector3(0.25f, 0.25f, -0.25f);
        [SerializeField] public Vector3 recoilRotation = new Vector3(12.0f, 6.0f, 12.0f);
        [SerializeField] public float weaponRecoilReturnSpeed = 7f;
        [Space]
        [SerializeField] public float rotationSpeed = 8f;
        [SerializeField] public float returnSpeed = 2f;

        [Space]
        [SerializeField] private Vector3 recoilTargetRotation;
        [SerializeField] private Vector3 recoilCurrentRotation;
        [SerializeField] private Vector3 weaponRecoilOffset;
        [SerializeField] private Vector3 weaponCurrentOffset;
        [SerializeField] private CameraFOVHandler cameraFOVHandler;

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

        private void FixedUpdate()
        {
            // CAMERA RECOIL
            recoilTargetRotation = Vector3.Lerp(
                recoilTargetRotation, Vector3.zero,
                Time.fixedDeltaTime * returnSpeed
            );

            recoilCurrentRotation = Vector3.Slerp(
                recoilCurrentRotation, recoilTargetRotation,
                Time.fixedDeltaTime * rotationSpeed
            );

            if (recoilCamera != null)
            {
                recoilCamera.localRotation = Quaternion.Euler(recoilCurrentRotation);
            }

            // WEAPON RECOIL OFFSET
            weaponRecoilOffset = Vector3.Lerp(
                weaponRecoilOffset, Vector3.zero,
                Time.fixedDeltaTime * weaponRecoilReturnSpeed
            );

            weaponCurrentOffset = Vector3.Slerp(
                weaponCurrentOffset, weaponRecoilOffset,
                Time.fixedDeltaTime * weaponRecoilReturnSpeed
            );

            if (weaponTransform != null) { weaponTransform.localPosition = weaponCurrentOffset; }
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
                recoilTargetRotation += new Vector3(
                    -recoilRotation.x,
                    Random.Range(-recoilRotation.y, recoilRotation.y),
                    Random.Range(-recoilRotation.z, recoilRotation.z)
                );

                weaponRecoilOffset += new Vector3(
                    weaponRecoilKick.x,
                    Random.Range(
                        -weaponRecoilKick.y,
                         weaponRecoilKick.y
                    ),
                    weaponRecoilKick.z
                );

                Debug.Log($"{gunData.WeaponName} hit {hit.collider.name}");

                if (hitEffectPrefab != null)
                {
                    ParticleSystem hitEffect = Instantiate(
                        hitEffectPrefab, hit.point,
                        Quaternion.LookRotation(hit.normal)
                    );
                    Destroy(hitEffect.gameObject, 2f); // Destroy the hit effect after 2 seconds
                }

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