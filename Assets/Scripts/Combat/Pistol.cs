using UnityEngine;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class Pistol : Gun
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Update() { base.Update(); }

        /// <summary>
        /// 
        /// </summary>
        public override void Shoot()
        {
            RaycastHit hit;

            if (
                    Physics.Raycast
                    (
                        cameraTransform.position, 
                        cameraTransform.forward, 
                        out hit, gunData.range, 
                        gunData.targetLayerMask
                    )
               )
            {
                Debug.Log(gunData.weaponName + " hit " + hit.collider.name);
            }
        }
    }
}
