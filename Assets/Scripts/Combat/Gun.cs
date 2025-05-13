using System.Collections;
using UnityEngine;
using UrbanFracture.Core.Player;
using UrbanFracture.Player.Components;

namespace UrbanFracture.Combat
{
    public abstract class Gun : MonoBehaviour
    {
        public GunData gunData;
        [HideInInspector] public FirstPersonController firstPersonController;
        [HideInInspector] public Transform cameraTransform;
        [HideInInspector] public Recoil recoilHandler;

        private float currentAmmo = 0f;
        private float nextTimeToFire = 0f;

        private bool isReloading = false;

        private void Start()
        {
            currentAmmo = gunData.magazineSize;
            firstPersonController = transform.root.GetComponent<FirstPersonController>();
            cameraTransform = firstPersonController.firstPersonCamera.transform;
            recoilHandler = GetComponent<Recoil>();
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
                Debug.Log(gunData.weaponName + " is reloading...");
                return;
            }
            if (currentAmmo <= 0f)
            {
                Debug.Log(gunData.weaponName + " has run out of ammo, please reload...");
                return;
            }
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + (1 / gunData.fireRate);
                HandleShoot();
            }
        }

        private void HandleShoot()
        {
            currentAmmo--;
            Debug.Log(gunData.weaponName + " shot!, bullets left: " + currentAmmo);
            Shoot();
            recoilHandler?.ApplyRecoil(gunData);
        }

        public abstract void Shoot();

        public void TryReload() { if (!isReloading && currentAmmo < gunData.magazineSize) { StartCoroutine(Reload()); } }

        public IEnumerator Reload()
        {
            isReloading = true;

            Debug.Log(gunData.weaponName + " is reloading...");

            yield return new WaitForSeconds(gunData.reloadTime);

            currentAmmo = gunData.magazineSize;
            isReloading = false;

            Debug.Log(gunData.weaponName + " is reloaded...");
        }
    }
}
