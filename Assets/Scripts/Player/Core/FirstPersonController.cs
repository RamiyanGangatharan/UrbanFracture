using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UrbanFracture.Combat;
using UrbanFracture.Player.Components;
using UrbanFracture.UI.HUD;
using UrbanFracture.UI.MainMenu;

namespace UrbanFracture.Core.Player
{
    /// <summary>
    /// Controls first-person player behavior including movement, camera rotation,
    /// sprinting, jumping, and invoking events on landing.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] public CinemachineCamera firstPersonCamera;
        [SerializeField] private Footsteps footsteps;
        [SerializeField] private Canvas gameHUDCanvas;
        [SerializeField] private Health playerHealth;
        [SerializeField] private Transform cameraPivotTransform;

        public Health PlayerHealth => playerHealth;

        [Header("Input")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;
        public UnityEvent Landed;

        [Header("Combat")]
        [SerializeField] private Gun currentGun;

        [Header("Player Stats")]
        public Gun EquippedGun => currentGun;

        // Component Handlers
        private MovementHandler movementHandler;
        private LookHandler lookHandler;
        private CameraFOVHandler FOVHandler;
        private JumpHandler jumpHandler;
        private CrouchHandler crouchHandler;

        private GameHUD gameHUD;

        /// <summary>
        /// Ensures required component references are assigned in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (footsteps == null) footsteps = GetComponentInChildren<Footsteps>();
            if (gameHUDCanvas == null) gameHUDCanvas = GetComponentInChildren<Canvas>(); 
            if (playerHealth == null) playerHealth = GetComponent<Health>();
            if (crouchHandler == null) crouchHandler = GetComponent<CrouchHandler>();
        } 

        /// <summary>
        /// Initializes all gameplay-related handlers for movement, camera control, FOV, and jumping.
        /// </summary>
        private void Awake()
        {
            movementHandler = new MovementHandler(characterController, crouchHandler);
            crouchHandler = GetComponent<CrouchHandler>();
            lookHandler = new LookHandler(transform, cameraPivotTransform);
            FOVHandler = new CameraFOVHandler(firstPersonCamera);
            jumpHandler = new JumpHandler(characterController, crouchHandler);

            if (gameHUDCanvas != null) { gameHUD = gameHUDCanvas.GetComponentInChildren<GameHUD>(); }
        }

        /// <summary>
        /// Updates player movement, camera look direction, FOV adjustments, jumping, and landing events.
        /// </summary>
        private void Update()
        {
            if (!PauseMenuController.isPaused)
            {
                movementHandler.Update(moveInput, sprintInput);
                lookHandler.Update(lookInput);
                FOVHandler.Update(movementHandler.CurrentSpeed, sprintInput);
                jumpHandler.Update(ref movementHandler.verticalVelocity);
                crouchHandler.Update();
                if (jumpHandler.CheckLanding()) { Landed?.Invoke(); }
                movementHandler.ApplyMovement(movementHandler.verticalVelocity);

                footsteps.HandleFootsteps(movementHandler.CurrentSpeed, characterController.isGrounded);

                if (Keyboard.current.hKey.wasPressedThisFrame) { ToggleHolsterWeapon(); }
            }
            
        }

        public void TryJump() => jumpHandler.TryJump(ref movementHandler.verticalVelocity);

        void ToggleHolsterWeapon()
        {
            if (currentGun != null)
            {
                if (currentGun.IsHolstered()) { currentGun.UnholsterWeapon(); }
                else { currentGun.HolsterWeapon(); }
                gameHUD?.UpdateHUD();
            }
        }

        public void TryAttack()
        {
            if (currentGun != null)
            {
                currentGun.TryShoot();
                gameHUD?.UpdateHUD();
            }
        }
        public void TryReload()
        {
            if (currentGun != null)
            {
                currentGun.TryReload();
                gameHUD?.UpdateHUD();
            }
        }

        public void TryCrouch()
        {
            crouchHandler.ToggleCrouch();
        }

        public void EquipGun(Gun gun)
        {
            if (currentGun != null) { currentGun.HolsterWeapon(); }
            currentGun = gun;

            if (currentGun != null) { currentGun.UnholsterWeapon(); }
            gameHUD?.UpdateHUD();
        }

        public void TakeDamage(float amount)
        {
            playerHealth?.TakeDamage(amount);
            gameHUD?.UpdateHUD();
        }
    }
}
