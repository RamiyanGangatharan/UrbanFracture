using UnityEngine;

namespace UrbanFracture.Combat
{
    public class Pistol : Gun
    {
        [Header("Effects")]
        public ParticleSystem muzzleFlash;
        public Transform muzzleFlashSpawnPoint;
        public ParticleSystem hitEffectParticleSystem;

        public override void Update()
        {
            base.Update();
        }

        public override void Shoot()
        {
            // Muzzle flash
            if (muzzleFlash != null)
            {
                if (muzzleFlashSpawnPoint != null)
                {
                    muzzleFlash.transform.position = muzzleFlashSpawnPoint.position;
                    muzzleFlash.transform.rotation = muzzleFlashSpawnPoint.rotation;
                }

                muzzleFlash.Play();
            }

            // Raycast hit
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, gunData.Range, gunData.TargetLayerMask))
            {
                Debug.Log($"{gunData.WeaponName} hit {hit.collider.name}");

                if (hitEffectParticleSystem != null)
                {
                    var hitEffect = Instantiate(hitEffectParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                    hitEffect.Play();

                    float destroyDelay = hitEffect.main.duration + hitEffect.main.startLifetime.constantMax;
                    Destroy(hitEffect.gameObject, destroyDelay);
                }
            }
        }
    }
}
