using UnityEngine;
using UnityEngine.InputSystem;

namespace UrbanFracture.Combat
{
    public class WeaponADS : MonoBehaviour
    {
        [SerializeField] private Canvas Crosshair;
        [SerializeField] private Transform HipFirePosition;
        [SerializeField] private Transform ADSPosition;
        [SerializeField] private Transform MuzzleFlashPosition;
        [SerializeField] private Transform HipFireMuzzle;
        [SerializeField] private Transform ADSMuzzle;
        [SerializeField] private float aimSpeed = 10f;

        private bool isAiming;

        public bool IsAiming => isAiming;

        void Update()
        {
            if (HipFirePosition == null || ADSPosition == null)
            {
                Debug.LogWarning("ADS positions not assigned.");
                return;
            }

            // Set the field, not the property!
            isAiming = Mouse.current.rightButton.isPressed;

            Transform target = isAiming ? ADSPosition : HipFirePosition;
            Transform muzzleTarget = isAiming ? ADSMuzzle : HipFireMuzzle;

            if (isAiming)
            {
                Crosshair.enabled = false;
            }
            else
            {
                Crosshair.enabled = true;
            }

            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * aimSpeed
            );

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                target.rotation,
                Time.deltaTime * aimSpeed
            );

            if (MuzzleFlashPosition != null && muzzleTarget != null)
            {
                MuzzleFlashPosition.position = Vector3.Lerp(
                    MuzzleFlashPosition.position,
                    muzzleTarget.position,
                    Time.deltaTime * aimSpeed
                );

                MuzzleFlashPosition.rotation = Quaternion.Lerp(
                    MuzzleFlashPosition.rotation,
                    muzzleTarget.rotation,
                    Time.deltaTime * aimSpeed
                );
            }
        }
    }
}