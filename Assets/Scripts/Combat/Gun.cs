using System.Collections;
using UnityEngine;
using UrbanFracture.Core.Player;
using UrbanFracture.Player.Components;
using UrbanFracture.UI.HUD;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Abstract base class for all guns in the game. 
    /// Handles common weapon logic such as shooting, reloading, recoil, and HUD updates.
    /// </summary>
    public abstract class Gun : MonoBehaviour
    {
        public GunData gunData;
        [HideInInspector] public FirstPersonController firstPersonController;
        [HideInInspector] public Transform cameraTransform;
        [HideInInspector] public Recoil recoilHandler;

        public GameHUD gameHUD;

        public float currentAmmo = 0f;
        private float nextTimeToFire = 0f;
        private bool isReloading = false;

        [Header("Audio Sources")]
        [Tooltip("AudioSource component that plays shooting sound.")]
        public AudioSource shootSFX;

        [Tooltip("AudioSource component that plays reloading sound.")]
        public AudioSource reloadSFX;

        /// <summary>
        /// Initializes references to player controller, camera, recoil handler, and HUD. 
        /// Sets the current ammo to the magazine size.
        /// </summary>
        private void Start()
        {
            currentAmmo = gunData.MagazineSize;
            firstPersonController = transform.root.GetComponent<FirstPersonController>();
            cameraTransform = firstPersonController.firstPersonCamera.transform;
            recoilHandler = GetComponent<Recoil>();

            if (firstPersonController != null) { gameHUD = firstPersonController.GetComponentInChildren<GameHUD>(); }
        }

        /// <summary>
        /// Updates gun state each frame. Resets recoil when applicable.
        /// </summary>
        public virtual void Update() { recoilHandler?.ResetRecoil(gunData); }

        /// <summary>
        /// Attempts to fire the gun if it's not reloading and has ammo. 
        /// Applies fire rate cooldown.
        /// </summary>
        public void TryShoot()
        {
            if (isReloading) { Debug.Log($"{gunData.WeaponName} is reloading..."); return; }
            if (currentAmmo <= 0f) { Debug.Log($"{gunData.WeaponName} has run out of ammo, please reload..."); return; }
            if (Time.time >= nextTimeToFire) { nextTimeToFire = Time.time + (1 / gunData.FireRate); HandleShoot(); }
        }

        /// <summary>
        /// Handles the internal shooting logic by decrementing the ammo count, 
        /// playing the shooting sound effect, triggering the abstract Shoot() method 
        /// (which is implemented by specific gun types), and applying recoil using 
        /// the associated recoil handler.
        /// </summary>
        private void HandleShoot()
        {
            currentAmmo--;

            Debug.Log($"{gunData.WeaponName} shot! Bullets left: {currentAmmo}");

            shootSFX?.Play();
            Shoot();

            recoilHandler?.ApplyRecoil(gunData);
        }

        /// <summary>
        /// Abstract method to be overridden by specific gun types.
        /// Defines behavior for shooting (e.g., raycasting, effects).
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// Attempts to reload the weapon if not already reloading and magazine isn't full.
        /// </summary>
        public void TryReload()
        {
            if (!isReloading && currentAmmo < gunData.MagazineSize) { StartCoroutine(Reload()); }
        }

        /// <summary>
        /// Coroutine that performs reload behavior:
        /// - Plays reload sound
        /// - Waits for reload time
        /// - Resets ammo count
        /// - Updates HUD
        /// </summary>
        /// <returns>IEnumerator for coroutine execution</returns>
        public IEnumerator Reload()
        {
            isReloading = true;

            Debug.Log($"{gunData.WeaponName} is reloading...");
            reloadSFX?.Play();

            yield return new WaitForSeconds(gunData.ReloadTime);

            currentAmmo = gunData.MagazineSize;
            isReloading = false;

            Debug.Log($"{gunData.WeaponName} is reloaded.");
            gameHUD?.UpdateHUD();
        }
    }
}
