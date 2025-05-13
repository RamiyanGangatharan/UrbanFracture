using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using UrbanFracture.Player.Components;

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
        [SerializeField] private CinemachineCamera firstPersonCamera;
        [SerializeField] private Footsteps footsteps;


        [Header("Input")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;
        public UnityEvent Landed;

        // Component Handlers
        private MovementHandler movementHandler;
        private LookHandler lookHandler;
        private CameraFOVHandler FOVHandler;
        private JumpHandler jumpHandler;
        private CameraBob cameraBob;

        /// <summary>
        /// Ensures required component references are assigned in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (footsteps == null) footsteps = GetComponentInChildren<Footsteps>();
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

            cameraBob = GetComponent<CameraBob>();
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
            cameraBob.UpdateCameraBob(movementHandler.CurrentSpeed, characterController.isGrounded);
        }

        /// <summary>
        /// Attempts to initiate a jump using the current vertical velocity state.
        /// </summary>
        public void TryJump() => jumpHandler.TryJump(ref movementHandler.verticalVelocity);
    }
}
