using System.Collections;
using UnityEngine;
using UrbanFracture.Core.Player;
using UrbanFracture.UI.HUD;
using UrbanFracture.UI.MainMenu;

namespace UrbanFracture.Combat
{
    /// <summary>
    /// Abstract base class for all guns in the game. 
    /// Handles common weapon logic.
    /// </summary>
    public abstract class Gun : MonoBehaviour
    {
        public GunData gunData;
        [HideInInspector] public FirstPersonController firstPersonController;
        [HideInInspector] public Transform cameraTransform;

        public GameHUD gameHUD;

        public float currentAmmo = 0f;
        private float nextTimeToFire = 0f;
        private bool isReloading = false;
        private bool isHolstered = false;

        public bool IsHolstered() => isHolstered;

        [Header("Audio Sources")]
        public AudioSource shootSFX;
        public AudioSource reloadSFX;
        public AudioSource emptyMagazineSFX;
        public AudioSource holsterWeaponSFX;

        private void Start()
        {
            currentAmmo = gunData.MagazineSize;
            firstPersonController = transform.root.GetComponent<FirstPersonController>();
            cameraTransform = firstPersonController.firstPersonCamera.transform;

            if (firstPersonController != null)
            {
                gameHUD = firstPersonController.GetComponentInChildren<GameHUD>();
            }
        }

        public virtual void Update() { }

        /// <summary>
        /// Attempts to fire the gun if it's not reloading and has ammo. 
        /// Applies fire rate cooldown.
        /// </summary>
        public void TryShoot()
        {
            if (!PauseMenuController.isPaused)
            {
                if (isHolstered) return;
                if (isReloading) { Debug.Log($"{gunData.WeaponName} is reloading..."); return; }
                if (currentAmmo <= 0f)
                {
                    Debug.Log($"{gunData.WeaponName} is out of ammo...");
                    emptyMagazineSFX?.PlayOneShot(emptyMagazineSFX.clip);
                    return;
                }
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + (1 / gunData.FireRate);
                    HandleShoot();
                }
            }
        }

        private void PlayGunshot()
        {
            if (shootSFX != null && shootSFX.clip != null) { shootSFX.PlayOneShot(shootSFX.clip); }
            else { Debug.LogWarning("Shoot SFX not assigned or missing clip."); }
        }

        /// <summary>
        /// Handles the internal shooting logic by decrementing the ammo count, 
        /// playing the shooting sound effect, triggering the abstract Shoot() method 
        /// (which is implemented by specific gun types), and applying recoil using 
        /// the associated recoil handler.
        /// </summary>
        private void HandleShoot()
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                Debug.Log($"{gunData.WeaponName} shot! Bullets left: {currentAmmo}");
                PlayGunshot();
                Shoot();
            }
            else
            {
                Debug.Log($"{gunData.WeaponName} is out of ammo...");
                emptyMagazineSFX?.PlayOneShot(emptyMagazineSFX.clip);
            }
        }

        /// <summary>
        /// Abstract method to be overridden by specific gun types.
        /// Defines behavior for shooting (e.g., raycasting, effects).
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// NEW unified method for holstering/unholstering.
        /// This method sets the holstered state of the gun.
        /// </summary>
        /// <param name="shouldHolster"></param>
        public void SetHolstered(bool shouldHolster)
        {
            if (!PauseMenuController.isPaused)
            {
                isHolstered = shouldHolster;
                gameObject.SetActive(!shouldHolster);
                Debug.Log($"{gunData.WeaponName} {(shouldHolster ? "holstering..." : "unholstering...")}");
                holsterWeaponSFX?.Play();
            }
        }

        /// <summary>
        /// Attempts to reload the weapon if not already reloading and magazine isn't full.
        /// </summary>
        public void TryReload()
        {
            if (!PauseMenuController.isPaused)
            {
                if (isHolstered) { return; }
                if (!isReloading && currentAmmo < gunData.MagazineSize) { StartCoroutine(Reload()); }
            }
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
            if (!PauseMenuController.isPaused)
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
}
