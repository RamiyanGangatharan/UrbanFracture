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
    /// This is a controller that controls the player that the user controls.
    /// For all XML documentation, refer to the base class.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : BaseCharacterController
    {
        [Header("References")]
        [SerializeField] public CinemachineCamera firstPersonCamera;
        [SerializeField] private Footsteps footsteps;
        [SerializeField] private Canvas gameHUDCanvas;
        [SerializeField] private Transform cameraPivotTransform;
        [SerializeField] private LeanHandler leanHandler;

        [Header("Input")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;
        public UnityEvent Landed;

        private LookHandler lookHandler;
        private CameraFOVHandler FOVHandler;
        private GameHUD gameHUD;

        public Gun EquippedGun => currentGun;
        public Health PlayerHealth => characterHealth;

        protected override void Awake()
        {
            base.Awake();
            crouchHandler = GetComponent<CrouchHandler>();
            leanHandler = GetComponentInChildren<LeanHandler>();
            gameHUD = gameHUDCanvas.GetComponentInChildren<GameHUD>();
            lookHandler = new LookHandler(transform, cameraPivotTransform);
            FOVHandler = new CameraFOVHandler(firstPersonCamera);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (footsteps == null) footsteps = GetComponentInChildren<Footsteps>();
            if (gameHUDCanvas == null) gameHUDCanvas = GetComponentInChildren<Canvas>();
            if (leanHandler == null) leanHandler = GetComponentInChildren<LeanHandler>();
        }

        private void Update()
        {
            if (PauseMenuController.isPaused) return;

            ApplyMovement(moveInput, sprintInput);
            lookHandler.Update(lookInput);
            FOVHandler.Update(movementHandler.CurrentSpeed, sprintInput);

            footsteps.HandleFootsteps(movementHandler.CurrentSpeed, characterController.isGrounded);

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                ToggleHolsterWeapon();
                gameHUD?.UpdateHUD();
            }
        }

        public override void TryJump() { base.TryJump(); }

        public override void TryAttack()
        {
            base.TryAttack();
            gameHUD?.UpdateHUD();
        }

        public override void TryReload()
        {
            base.TryReload();
            gameHUD?.UpdateHUD();
        }

        public override void TryCrouch() { base.TryCrouch(); }

        public override void EquipGun(Gun gun)
        {
            base.EquipGun(gun);
            gameHUD?.UpdateHUD();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            gameHUD?.UpdateHUD();
        }

        public void TryLean(LeanHandler.LeanDirection direction)
        {
            Debug.Log($"Trying to lean: {direction}");
            leanHandler?.SetLean(direction);
        }

        public override void ToggleHolsterWeapon()
        {
            base.ToggleHolsterWeapon();
            gameHUD?.UpdateHUD();
        }

        public override void ApplyMovement(Vector2 moveInput, bool sprintInput)
        {
            base.ApplyMovement(moveInput, sprintInput);
            if (jumpHandler.CheckLanding()) Landed?.Invoke();
        }
    }
}
