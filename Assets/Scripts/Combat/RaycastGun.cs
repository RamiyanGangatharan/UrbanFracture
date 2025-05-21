using UnityEngine;
using System.Collections;

namespace UrbanFracture.Combat
{
    public class RaycastGun : Gun
    {
        [Header("Raycast Settings")]
        public float range = 100f;
        public float impactForce = 50f;
        public GameObject hitEffectPrefab;
        public TrailRenderer bulletTrailPrefab;

        public override void Shoot()
        {
            Vector3 origin = cameraTransform.position;
            Vector3 direction = cameraTransform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
            {
                // Damage handling
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float damage = gunData.Damage; // or calculate based on distance
                    damageable.TakeDamage(damage);
                }

                // Impact physics
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
                }

                // Impact effect
                if (hitEffectPrefab)
                {
                    Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }

                // Visual bullet trail
                if (bulletTrailPrefab != null)
                {
                    StartCoroutine(SpawnTrail(origin, hit.point));
                }
            }
            else
            {
                // Trail to max distance if no hit
                if (bulletTrailPrefab != null)
                {
                    StartCoroutine(SpawnTrail(origin, origin + direction * range));
                }
            }
        }

        private IEnumerator SpawnTrail(Vector3 start, Vector3 end)
        {
            TrailRenderer trail = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
            float time = 0f;
            float duration = 0.05f;
            while (time < 1f)
            {
                trail.transform.position = Vector3.Lerp(start, end, time);
                time += Time.deltaTime / duration;
                yield return null;
            }

            trail.transform.position = end;
            Destroy(trail.gameObject, trail.time);
        }
    }
}
