using UnityEngine;
using UrbanFracture.Combat;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// This class is responsible for controlling recoil on a weapon
    /// </summary>
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
            if (cameraTransform == null) { cameraTransform = Camera.main.transform; }
        }

        /// <summary>
        /// Updates the recoil effect by gradually resetting the current recoil value towards zero.
        /// The recoil is smoothed over time using <see cref="Vector3.SmoothDamp"/> to create a natural recoil reset.
        /// </summary>
        private void Update()
        {
            currentRecoil = Vector3.SmoothDamp(
                currentRecoil, 
                Vector3.zero, 
                ref currentVelocity, 
                1f / gunData.ResetRecoilSpeed
            );
        }

        /// <summary>
        /// Applies the recoil to the camera's local rotation by modifying it with the current recoil offset.
        /// This is done in the LateUpdate method to ensure the recoil is applied after all other updates 
        /// have occurred, resulting in smoother camera behavior.
        /// </summary>
        private void LateUpdate()
        {
            if (cameraTransform != null)
            {
                cameraTransform.localRotation *= Quaternion.Euler(currentRecoil);
            }
        }

        /// <summary>
        /// Applies recoil to the weapon's aim based on the gun's recoil data.
        /// The recoil is applied randomly within the specified range for the X and Y axes, 
        /// then smoothly moves the current recoil towards the target recoil value over time.
        /// </summary>
        /// <param name="gunData">
        /// data of the gun, containing recoil parameters such as 
        /// maximum recoil, recoil amount, and speed.
        /// </param>
        public void ApplyRecoil(GunData gunData)
        {
            float recoilX = Random.Range(-gunData.MaximumRecoil.x, gunData.MaximumRecoil.x) * gunData.RecoilAmount;
            float recoilY = Random.Range(-gunData.MaximumRecoil.y, gunData.MaximumRecoil.y) * gunData.RecoilAmount;

            targetRecoil += new Vector3(recoilX, recoilY, 0);
            currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.RecoilSpeed);
        }

        /// <summary>
        /// Resets the current recoil and target recoil to zero, gradually smoothing the recoil 
        /// back to its initial state. This is used to return the weapon's aim back to its 
        /// default position after shooting.
        /// </summary>
        /// <param name="gunData">
        /// The data of the gun, containing the speed at which recoil should reset.
        /// </param>
        public void ResetRecoil(GunData gunData)
        {
            currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.ResetRecoilSpeed);
            targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.ResetRecoilSpeed);
        }
    }
}
