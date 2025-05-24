using UnityEngine;
using System.Collections;

namespace UrbanFracture.Combat
{
    public class RaycastGun : Gun
    {
        [Header("Raycast Settings")]
        public float range = 100f;
        public float impactForce = 50f;

        public override void Shoot()
        {
            Vector3 origin = cameraTransform.position;
            Vector3 direction = cameraTransform.forward; 

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
            {
                // Hit detected - you can add custom logic here if needed
                Debug.Log($"Hit object: {hit.collider.name} at {hit.point}");
            }
            else
            {
                // No hit detected
                Debug.Log("No hit");
            }
        }
    }
}
