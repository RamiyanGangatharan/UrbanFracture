using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UrbanFracture.Combat;
using UrbanFracture.Player.Components;
using UrbanFracture.UI.HUD;

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
        [SerializeField] private Canvas gameHUDCanvas; // Canvas reference for GameHUD
        [SerializeField] private Health playerHealth;
        public Health PlayerHealth => playerHealth; // Expose for GameHUD

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

        private GameHUD gameHUD; // Reference to the GameHUD component

        /// <summary>
        /// Ensures required component references are assigned in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (footsteps == null) footsteps = GetComponentInChildren<Footsteps>();
            if (gameHUDCanvas == null) gameHUDCanvas = GetComponentInChildren<Canvas>(); // Find canvas if not assigned
            if (playerHealth == null) playerHealth = GetComponent<Health>();
        }

        /// <summary>
        /// Initializes all gameplay-related handlers for movement, camera control, FOV, and jumping.
        /// </summary>
        private void Awake()
        {
            movementHandler = new MovementHandler(characterController);
            lookHandler = new LookHandler(transform, firstPersonCamera);
            FOVHandler = new CameraFOVHandler(firstPersonCamera);
            jumpHandler = new JumpHandler(characterController);

            // Initialize GameHUD using the assigned canvas
            if (gameHUDCanvas != null)
            {
                gameHUD = gameHUDCanvas.GetComponentInChildren<GameHUD>();
            }
        }

        /// <summary>
        /// Updates player movement, camera look direction, FOV adjustments, jumping, and landing events.
        /// </summary>
        private void Update()
        {
            movementHandler.Update(moveInput, sprintInput);
            lookHandler.Update(lookInput);
            FOVHandler.Update(movementHandler.CurrentSpeed, sprintInput);
            jumpHandler.Update(ref movementHandler.verticalVelocity);

            if (jumpHandler.CheckLanding()) { Landed?.Invoke(); }
            movementHandler.ApplyMovement(movementHandler.verticalVelocity);

            footsteps.HandleFootsteps(movementHandler.CurrentSpeed, characterController.isGrounded);
        }

        public void TryJump() => jumpHandler.TryJump(ref movementHandler.verticalVelocity);
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

        public void EquipGun(Gun gun) => currentGun = gun;

        public void TakeDamage(float amount)
        {
            playerHealth?.TakeDamage(amount);
            gameHUD?.UpdateHUD(); // Update the HUD when damage is taken
        }
    }
}
