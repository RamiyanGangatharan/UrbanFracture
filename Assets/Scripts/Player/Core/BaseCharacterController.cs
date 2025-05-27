using UnityEngine;
using UnityEngine.Events;
using UrbanFracture.Combat;
using UrbanFracture.Player.Components;

namespace UrbanFracture.Core.Player
{
    /// <summary>
    /// Abstract base class for all character controllers in the game.
    /// Handles movement, jumping, crouching, and weapon interactions.
    /// </summary>
    public abstract class BaseCharacterController : MonoBehaviour
    {
        [Header("Core References")]
        [SerializeField] protected CharacterController characterController;
        [SerializeField] protected Health characterHealth;
        [SerializeField] protected Gun currentGun;

        [Header("Combat")]
        public Gun equippedGun => currentGun;

        [Header("Input")]
        UnityEvent Landed;

        // Movement-related handlers
        protected MovementHandler movementHandler;
        protected JumpHandler jumpHandler;
        protected CrouchHandler crouchHandler;

        /// <summary>
        /// Called on object initialization. Initializes core movement handlers.
        /// </summary>
        protected virtual void Awake()
        {
            crouchHandler = GetComponent<CrouchHandler>();
            movementHandler = new MovementHandler(characterController, crouchHandler);
            jumpHandler = new JumpHandler(characterController, crouchHandler);
        }

        /// <summary>
        /// Ensures that required component references are assigned in the editor or at runtime.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (characterHealth == null) characterHealth = GetComponent<Health>();
            if (crouchHandler == null) crouchHandler = GetComponent<CrouchHandler>();
        }

        /// <summary>
        /// Equips a new gun to the character and holsters the previous one if necessary.
        /// </summary>
        /// <param name="gun">The new gun to equip.</param>
        public virtual void EquipGun(Gun gun)
        {
            if (currentGun != null) currentGun.SetHolstered(true);
            currentGun = gun;
            if (currentGun != null) currentGun.SetHolstered(false);
        }

        /// <summary>
        /// Applies damage to the character's health.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        public virtual void TakeDamage(float amount) { characterHealth?.TakeDamage(amount); }

        /// <summary>
        /// Attempts to fire the currently equipped weapon.
        /// </summary>
        public virtual void TryAttack() { currentGun?.TryShoot(); }

        /// <summary>
        /// Attempts to reload the currently equipped weapon.
        /// </summary>
        public virtual void TryReload() { currentGun?.TryReload(); }

        /// <summary>
        /// Attempts to make the character jump.
        /// </summary>
        public virtual void TryJump() { if (jumpHandler != null) { jumpHandler.TryJump(ref movementHandler.verticalVelocity); } }

        /// <summary>
        /// Toggles the crouch state of the character.
        /// </summary>
        public virtual void TryCrouch() { crouchHandler?.ToggleCrouch(); }

        /// <summary>
        /// Toggles the holstered state of the currently equipped weapon.
        /// </summary>
        public virtual void ToggleHolsterWeapon()
        {
            if (currentGun == null) return;
            currentGun.SetHolstered(!currentGun.IsHolstered());
        }

        /// <summary>
        /// Applies movement input to the character including sprinting, jumping, and landing checks.
        /// </summary>
        /// <param name="moveInput">Directional input for movement (X = horizontal, Y = vertical).</param>
        /// <param name="sprintInput">Whether the character is currently sprinting.</param>
        public virtual void ApplyMovement(Vector2 moveInput, bool sprintInput)
        {
            movementHandler.Update(moveInput, sprintInput);
            jumpHandler.Update(ref movementHandler.verticalVelocity);

            if (jumpHandler.CheckLanding()) { Landed?.Invoke(); }

            crouchHandler?.Update();
            movementHandler.ApplyMovement(movementHandler.verticalVelocity);
        }
    }
}
