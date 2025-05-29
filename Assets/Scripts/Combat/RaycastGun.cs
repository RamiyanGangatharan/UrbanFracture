using UnityEngine;

namespace UrbanFracture.Combat
{
    public class RaycastGun : Gun
    {
        [Header("Raycast Settings")]
        public float range = 100f;
        public float impactForce = 50f;

        [Tooltip("If true, limits vertical knockback")]
        public bool limitVerticalForce = true;

        [Tooltip("Randomness added to force direction for natural variation")]
        [Range(0f, 0.3f)]
        public float forceDirectionRandomness = 0.1f;

        public override void Shoot()
        {
            Vector3 origin = cameraTransform.position;
            Vector3 direction = cameraTransform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
            {
                Debug.Log($"Hit object: {hit.collider.name} at {hit.point}");

                Rigidbody hitRb = hit.rigidbody;
                if (hitRb != null)
                {
                    // OPTIONAL: Check if the hit rigidbody is part of a ragdoll
                    // Example: ragdolls tagged as "EnemyRagdoll" or have a RagdollPhysics component
                    var ragdollPhysics = hitRb.GetComponentInParent<UrbanFracture.Player.Components.RagdollPhysics>();
                    if (ragdollPhysics == null)
                    {
                        Debug.Log("Hit object is not ragdolled, skipping force application.");
                        return;
                    }

                    // Direction from shooter to hit point (away from shooter)
                    Vector3 forceDir = (hit.point - origin).normalized;

                    // Add slight random spread to force direction for realism
                    if (forceDirectionRandomness > 0f)
                    {
                        forceDir += new Vector3(
                            Random.Range(-forceDirectionRandomness, forceDirectionRandomness),
                            Random.Range(-forceDirectionRandomness, forceDirectionRandomness),
                            Random.Range(-forceDirectionRandomness, forceDirectionRandomness)
                        );
                        forceDir.Normalize();
                    }

                    if (limitVerticalForce)
                    {
                        // Clamp vertical force without re-normalizing to avoid unintended scaling
                        forceDir.y = Mathf.Clamp(forceDir.y, -0.1f, 0.2f);
                        // Don't normalize again here!
                    }

                    // Optionally clamp overall force magnitude to prevent excessive impulses
                    float forceMagnitude = impactForce;
                    forceMagnitude = Mathf.Clamp(forceMagnitude, 0f, 30f); // adjust max force as needed

                    Vector3 finalForce = forceDir * forceMagnitude;

                    Debug.Log($"Applying force {finalForce} at {hit.point} to {hitRb.name}");

                    // Apply impulse force at hit position for torque + linear effect
                    hitRb.AddForceAtPosition(finalForce, hit.point, ForceMode.Impulse);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }

    }
}
