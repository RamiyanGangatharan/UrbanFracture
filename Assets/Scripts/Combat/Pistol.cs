using UnityEngine;

namespace UrbanFracture.Combat
{
    public class Pistol : Gun
    {
        public GameObject muzzleFlashPrefab;
        public GameObject hitEffectPrefab;

        public override void Update() { base.Update(); }

        public override void Shoot()
        {
            Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);

            RaycastHit hit;

            if (
                Physics.Raycast
                (
                    cameraTransform.position, 
                    cameraTransform.forward, 
                    out hit, gunData.Range, 
                    gunData.TargetLayerMask
                )
               )
            {
                Debug.Log(gunData.WeaponName + " hit " + hit.collider.name);

                if (hitEffectPrefab != null) 
                { 
                    Instantiate
                    (
                        hitEffectPrefab, 
                        hit.point, 
                        Quaternion.LookRotation(hit.normal)
                    ); 
                }
            }
        }
    }
}
