using UnityEngine;

namespace UrbanFracture.Combat
{
    public class RecoilHandler : MonoBehaviour
    {
        [Header("References")]
        public Transform recoilCamera;
        public Transform weaponTransform;
        public WeaponADS weaponADS; 

        [Header("Recoil Settings")]
        public Vector3 HipFireRecoilKick = new Vector3(0.1f, 0.1f, -0.1f);
        public Vector3 HipFireRotation = new Vector3(2f, 1f, 4f);

        public Vector3 adsRecoilKick = new Vector3(0.05f, 0.05f, -0.05f);
        public Vector3 adsRotation = new Vector3(1f, 0.5f, 2f);

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
            bool aiming = weaponADS != null && weaponADS.IsAiming;

            Vector3 rot = aiming ? adsRotation : HipFireRotation;
            Vector3 kick = aiming ? adsRecoilKick : HipFireRecoilKick;

            recoilTargetRotation += new Vector3(
                -rot.x,
                Random.Range(-rot.y, rot.y),
                Random.Range(-rot.z, rot.z)
            );

            weaponRecoilOffset += new Vector3(
                kick.x,
                Random.Range(-kick.y, kick.y),
                kick.z
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
