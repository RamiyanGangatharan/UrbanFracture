using UnityEngine;

namespace UrbanFracture.Combat
{
    public class RecoilHandler : MonoBehaviour
    {
        [Header("References")]
        public Transform recoilCamera;
        public Transform weaponTransform;

        [Header("Recoil Settings")]
        public Vector3 weaponRecoilKick = new Vector3(0.1f, 0.1f, -0.1f);
        public Vector3 recoilRotation = new Vector3(2f, 1f, 4f);
        public float weaponRecoilReturnSpeed = 7f;
        public float rotationSpeed = 7f;
        public float returnSpeed = 2f;

        [Header("RECOIL DEBUG - DO NOT TOUCH")]
        [SerializeField] private Vector3 recoilTargetRotation;
        [SerializeField] private Vector3 recoilCurrentRotation;
        [SerializeField] private Vector3 weaponRecoilOffset;
        [SerializeField] private Vector3 weaponCurrentOffset;

        public void ApplyRecoil()
        {
            recoilTargetRotation += new Vector3(
                -recoilRotation.x,
                Random.Range(-recoilRotation.y, recoilRotation.y),
                Random.Range(-recoilRotation.z, recoilRotation.z)
            );

            weaponRecoilOffset += new Vector3(
                weaponRecoilKick.x,
                Random.Range(-weaponRecoilKick.y, weaponRecoilKick.y),
                weaponRecoilKick.z
            );
        }

        public void Tick(float deltaTime)
        {
            recoilTargetRotation = Vector3.Lerp(
                recoilTargetRotation, 
                Vector3.zero, 
                deltaTime * returnSpeed
            );

            recoilCurrentRotation = Vector3.Slerp(
                recoilCurrentRotation, 
                recoilTargetRotation, 
                deltaTime * rotationSpeed
            );

            if (recoilCamera != null)
            {
                recoilCamera.localRotation = Quaternion.Euler(recoilCurrentRotation);
            }

            weaponRecoilOffset = Vector3.Lerp(
                weaponRecoilOffset, 
                Vector3.zero, 
                deltaTime * weaponRecoilReturnSpeed
            );

            weaponCurrentOffset = Vector3.Slerp(
                weaponCurrentOffset, 
                weaponRecoilOffset, 
                deltaTime * weaponRecoilReturnSpeed
            );

            if (weaponTransform != null)
            {
                weaponTransform.localPosition = weaponCurrentOffset;
            }
        }
    }
}
