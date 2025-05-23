using UnityEngine;
using UnityEngine.InputSystem;

namespace UrbanFracture.Core.Player
{
    [RequireComponent(typeof(FirstPersonController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FirstPersonController firstPersonController;

        void OnMove(InputValue value) => firstPersonController.moveInput = value.Get<Vector2>();
        void OnLook(InputValue value) => firstPersonController.lookInput = value.Get<Vector2>();
        void OnSprint(InputValue value) => firstPersonController.sprintInput = value.isPressed;
        void OnJump(InputValue value) { if (value.isPressed) firstPersonController.TryJump(); }
        void OnCrouch(InputValue value) { if (value.isPressed) firstPersonController.TryCrouch(); }
        void OnAttack(InputValue value) { if (value.isPressed) firstPersonController.TryAttack(); }
        void OnReload(InputValue value) { if (value.isPressed) firstPersonController.TryReload(); }

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
