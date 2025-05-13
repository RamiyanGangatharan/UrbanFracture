using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace UrbanFracture.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 15f;
        [SerializeField] private float acceleration = 15f;
        [Space(15)]
        [Tooltip("This is how high the character can jump")]
        [SerializeField] private float jumpHeight = 2f;

        private int timesJumped = 0;
        [SerializeField] private bool canDoubleJump = true;

        [Header("Looking Parameters")]
        public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
        public float pitchLimit = 80f;
        [SerializeField] private float currentPitch = 0f;

        [Header("Camera Parameters")]
        [SerializeField] private float CameraNormalFOV = 60f;
        [SerializeField] private float CameraSprintFOV = 80f;
        [SerializeField] private float CameraFOVSmoothing = 1f;

        [Header("Physics Parameters")]
        [SerializeField] private float GravityScale = 3f;
        public Vector3 currentVelocity { get; private set; }
        public float currentSpeed { get; private set; }
        public bool isGrounded => characterController.isGrounded;

        private bool wasGrounded = false;
        public float verticalVelocity = 0f;

        [Header("Input")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;

        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CinemachineCamera firstPersonCamera;

        [Header("Events")]
        public UnityEvent Landed;

        public float maximumSpeed => sprintInput ? sprintSpeed : walkSpeed;
        public bool sprinting => sprintInput && currentSpeed > 0.1f;

        public float CurrentPitch
        {
            get => currentPitch;
            set => currentPitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
        }

        private void OnValidate()
        {
            if (characterController == null) { characterController = GetComponent<CharacterController>(); }
        }

        private void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate();

            if (!wasGrounded && isGrounded)
            {
                timesJumped = 0;
                Landed?.Invoke();
            }

            wasGrounded = isGrounded;
        }

        /// <summary>
        /// Handle player movement based off of keyboard input
        /// </summary>
        private void MoveUpdate()
        {
            Vector3 horizontalInput = transform.forward * moveInput.y + transform.right * moveInput.x;
            horizontalInput.Normalize();

            if (horizontalInput.sqrMagnitude >= 0.01f)
            {
                currentVelocity = Vector3.MoveTowards
                (
                    currentVelocity,
                    horizontalInput * maximumSpeed,
                    acceleration * Time.deltaTime
                );
            }
            else
            {
                currentVelocity = Vector3.MoveTowards
                (
                    currentVelocity,
                    Vector3.zero,
                    acceleration * Time.deltaTime
                );
            }

            // Apply gravity
            if (isGrounded && verticalVelocity < 0f) { verticalVelocity = -3f; }
            else
            {
                verticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
                verticalVelocity = Mathf.Clamp(verticalVelocity, -100f, 100f);
            }

            Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);
            characterController.Move(fullVelocity * Time.deltaTime);

            currentSpeed = currentVelocity.magnitude;
        }

        /// <summary>
        /// Handle player camera movement based off of mouse input.
        /// </summary>
        private void LookUpdate()
        {
            Vector2 adjustedLook = new Vector2
            (
                lookInput.x * lookSensitivity.x,
                lookInput.y * lookSensitivity.y
            );

            currentPitch -= adjustedLook.y;
            currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

            if (firstPersonCamera != null)
            { 
                firstPersonCamera.transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f); 
            }

            transform.Rotate(Vector3.up * adjustedLook.x);
        }

        /// <summary>
        /// This function changes field of view based on speed of character
        /// </summary>
        private void CameraUpdate()
        {
            if (firstPersonCamera == null) return;

            float targetFOV = sprinting ? Mathf.Lerp
            (
                CameraNormalFOV, CameraSprintFOV, 
                currentSpeed / sprintSpeed
            )
            : CameraNormalFOV;

            firstPersonCamera.Lens.FieldOfView = Mathf.Lerp
            (
                firstPersonCamera.Lens.FieldOfView,
                targetFOV, CameraFOVSmoothing * Time.deltaTime
            );
        }

        /// <summary>
        /// Attempts to perform a jump. Respects double-jump rules and ground checks.
        /// </summary>
        public void TryJump()
        {
            bool groundedJump = isGrounded && verticalVelocity <= 0f;
            bool doubleJumpAllowed = canDoubleJump && timesJumped < 2;
            bool singleJumpAllowed = !canDoubleJump && timesJumped < 1;

            bool validJump = groundedJump || doubleJumpAllowed || singleJumpAllowed;

            if (!validJump) return;

            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * GravityScale);
            timesJumped++;
        }
    }
}

