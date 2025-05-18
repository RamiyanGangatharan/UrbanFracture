using UnityEngine;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Stores configuration data for firearms, including recoil, firing, reload behavior, and damage.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/GunData")]
    public class GunData : ScriptableObject
    {
        [Header("General Info")]
        [Tooltip("Name of the weapon to display in UI.")]
        [SerializeField] private string weaponName;
        [SerializeField] private Texture weaponIcon;

        [Header("Targeting")]
        [Tooltip("Layers that the weapon can hit.")]
        [SerializeField] private LayerMask targetLayerMask;

        [Header("Firing Configuration")]
        [Tooltip("Effective range of the weapon.")]
        [SerializeField] private float range = 100f;

        [Tooltip("Rate of fire in shots per second.")]
        [SerializeField] private float fireRate = 5f;

        [Tooltip("How much damage the weapon deals per shot.")]
        [SerializeField] private float damage = 10f;

        [Header("Reload Configuration")]
        [Tooltip("Maximum bullets per magazine.")]
        [SerializeField] private float magazineSize = 12f;

        [Tooltip("Time it takes to reload (in seconds).")]
        [SerializeField] private float reloadTime = 1.5f;

        // Public accessors
        public string WeaponName => weaponName;
        public Texture WeaponIcon => weaponIcon;
        public LayerMask TargetLayerMask => targetLayerMask;
        public float Range => range;
        public float FireRate => fireRate;
        public float Damage => damage; 
        public float MagazineSize => magazineSize;
        public float ReloadTime => reloadTime;
    }
}
