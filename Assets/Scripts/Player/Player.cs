using UnityEngine;
using UnityEngine.InputSystem;

namespace UrbanFracture.Player
{
    [RequireComponent(typeof(FirstPersonController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FirstPersonController firstPersonController;

        // These functions link Unity's Input System to our player and its controller

        void OnMove(InputValue value) { firstPersonController.moveInput = value.Get<Vector2>(); }
        void OnLook(InputValue value) { firstPersonController.lookInput = value.Get<Vector2>(); }
        void OnSprint(InputValue value) { firstPersonController.sprintInput = value.isPressed; }
        void OnJump(InputValue value) { if (value.isPressed) { firstPersonController.TryJump(); } }

        private void OnValidate()
        {
            if (firstPersonController == null) { firstPersonController = GetComponent<FirstPersonController>(); }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
