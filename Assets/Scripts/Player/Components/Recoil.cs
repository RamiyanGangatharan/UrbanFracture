using UnityEngine;
using UrbanFracture.Combat;

namespace UrbanFracture.Player.Components
{
    public class Recoil : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private GunData gunData;

        [Header("Recoil")]
        private Vector3 targetRecoil = Vector3.zero;
        private Vector3 currentRecoil = Vector3.zero;
        private Vector3 currentVelocity = Vector3.zero;

        private void Start()
        {
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            currentRecoil = Vector3.SmoothDamp(currentRecoil, Vector3.zero, ref currentVelocity, 1f / gunData.ResetRecoilSpeed);
        }

        private void LateUpdate()
        {
            if (cameraTransform != null)
            {
                cameraTransform.localRotation *= Quaternion.Euler(currentRecoil);
            }
        }

        public void ApplyRecoil(GunData gunData)
        {
            float recoilX = Random.Range(-gunData.MaximumRecoil.x, gunData.MaximumRecoil.x) * gunData.RecoilAmount;
            float recoilY = Random.Range(-gunData.MaximumRecoil.y, gunData.MaximumRecoil.y) * gunData.RecoilAmount;

            targetRecoil += new Vector3(recoilX, recoilY, 0);
            currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.RecoilSpeed);
        }

        public void ResetRecoil(GunData gunData)
        {
            currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.ResetRecoilSpeed);
            targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.ResetRecoilSpeed);
        }
    }
}
