using UnityEngine;
using UrbanFracture.Combat;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class Recoil : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;

        [Header("Recoil")]
        private Vector3 targetRecoil = Vector3.zero;
        private Vector3 currentRecoil = Vector3.zero;

        private void Start() { if (cameraTransform == null) { cameraTransform = Camera.main.transform; } }

        private void LateUpdate()
        {
            if (cameraTransform != null) { cameraTransform.localRotation *= Quaternion.Euler(currentRecoil); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gunData"></param>
        public void ApplyRecoil(GunData gunData)
        {
            float recoilX = Random.Range(-gunData.maximumRecoil.x, gunData.maximumRecoil.x) * gunData.recoilAmount;
            float recoilY = Random.Range(-gunData.maximumRecoil.y, gunData.maximumRecoil.y) * gunData.recoilAmount;

            targetRecoil += new Vector3(recoilX, recoilY, 0);
            currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.recoilSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gunData"></param>
        public void ResetRecoil(GunData gunData)
        {
            currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
            targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
        }
    }
}
