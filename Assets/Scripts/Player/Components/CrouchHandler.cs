using UnityEngine;
using UrbanFracture.UI.HUD;

namespace UrbanFracture.Player.Components
{
    /// <summary>
    /// This class is responsible for allowing the player to crouch in game
    /// </summary>
    public class CrouchHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform weaponTransform;

        [Header("Crouch Settings")]
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float crouchingHeight = 1f;
        [SerializeField] private float crouchSpeed = 8f;

        [SerializeField] private Vector3 standingCameraPosition =  new Vector3(0.0f, 1.6f, 0.0f);
        [SerializeField] private Vector3 crouchingCameraPosition = new Vector3(0.0f, 1.0f, 0.0f);
        [SerializeField] private Vector3 weaponStandingPosition =  new Vector3(0.0f, 1.7f, 0.0f);
        [SerializeField] private Vector3 weaponCrouchingPosition = new Vector3(0.0f, 1.1f, 0.0f);

        private bool isCrouching = false;
      
        private Vector3 targetCameraPosition;
        private Vector3 targetWeaponPosition;

        public bool IsCrouching => isCrouching;

        public GameHUD gameHUD;

        private void Awake()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (cameraTransform == null) Debug.LogWarning("Camera Transform not set in CrouchHandler.");
            targetCameraPosition = standingCameraPosition;
            targetWeaponPosition = weaponStandingPosition;
        }

        /// <summary>
        /// This function determines whether or not a player is allowed to crouch and will toggle the crouch state.
        /// </summary>
        public void ToggleCrouch()
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? crouchingHeight : standingHeight;
            characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
            targetCameraPosition = isCrouching ? crouchingCameraPosition : standingCameraPosition;
            targetWeaponPosition = isCrouching ? weaponCrouchingPosition : weaponStandingPosition;

            if (gameHUD != null)
            {
                gameHUD?.UpdateHUD();
                gameHUD.SendMessage("UpdateStandState", SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// This will update the crouch state of the player in-game smoothly. It will be called every frame. 
        /// </summary>
        public void Update()
        {
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = Vector3.Lerp(
                    cameraTransform.localPosition,
                    targetCameraPosition,
                    Time.deltaTime * crouchSpeed
                );
            }

            if (weaponTransform != null)
            {
                weaponTransform.localPosition = Vector3.Lerp(
                    weaponTransform.localPosition,
                    targetWeaponPosition,
                    Time.deltaTime * crouchSpeed
                );
            }
        }
    }
}
