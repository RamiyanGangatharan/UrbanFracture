using UnityEngine;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/GunData")]
    public class GunData : ScriptableObject
    {
        public string weaponName;

        public float recoilSpeed;

        public LayerMask targetLayerMask;

        [Header("Firing Configuration")]
        public float range;
        public float fireRate;

        [Header("Reload Configuration")]
        public float magazineSize;
        public float reloadTime;

        [Header("Recoil Settings")]
        public float recoilAmount;
        public Vector2 maximumRecoil;
        
        public float resetRecoilSpeed;
    }
}

