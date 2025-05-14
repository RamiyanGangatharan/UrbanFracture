using System.Collections;
using UnityEngine;
using UrbanFracture.Core.Player;
using UrbanFracture.Player.Components;
using UrbanFracture.UI.HUD;

namespace UrbanFracture.Combat
{
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

        [Header("Audio")]
        public AudioClip shootSFX;
        public AudioClip reloadSFX;

        private AudioSource audioSource;

        private void Start()
        {
            currentAmmo = gunData.MagazineSize;
            firstPersonController = transform.root.GetComponent<FirstPersonController>();
            cameraTransform = firstPersonController.firstPersonCamera.transform;
            recoilHandler = GetComponent<Recoil>();

            if (firstPersonController != null)
            {
                gameHUD = firstPersonController.GetComponentInChildren<GameHUD>();
            }

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public virtual void Update()
        {
            if (recoilHandler != null)
            {
                recoilHandler.ResetRecoil(gunData);
            }
        }

        public void TryShoot()
        {
            if (isReloading)
            {
                Debug.Log(gunData.WeaponName + " is reloading...");
                return;
            }
            if (currentAmmo <= 0f)
            {
                Debug.Log(gunData.WeaponName + " has run out of ammo, please reload...");
                return;
            }
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + (1 / gunData.FireRate);
                HandleShoot();
            }
        }

        private void HandleShoot()
        {
            currentAmmo--;
            Debug.Log(gunData.WeaponName + " shot!, bullets left: " + currentAmmo);

            // 🔊 Play shoot SFX
            if (shootSFX != null && audioSource != null)
                audioSource.PlayOneShot(shootSFX);

            Shoot();
            recoilHandler?.ApplyRecoil(gunData);
        }

        public abstract void Shoot();

        public void TryReload()
        {
            if (!isReloading && currentAmmo < gunData.MagazineSize)
            {
                StartCoroutine(Reload());
            }
        }

        public IEnumerator Reload()
        {
            isReloading = true;

            if (gameHUD.ReloadText != null)
                gameHUD.ReloadText.enabled = true;

            Debug.Log(gunData.WeaponName + " is reloading...");

            // 🔊 Play reload SFX
            if (reloadSFX != null && audioSource != null)
                audioSource.PlayOneShot(reloadSFX);

            yield return new WaitForSeconds(gunData.ReloadTime);

            currentAmmo = gunData.MagazineSize;
            isReloading = false;

            Debug.Log(gunData.WeaponName + " is reloaded...");
            if (gameHUD.ReloadText != null)
                gameHUD.ReloadText.enabled = false;

            gameHUD?.UpdateHUD();
        }
    }
}
