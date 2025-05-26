using UnityEngine;
using UnityEngine.InputSystem;

namespace UrbanFracture.Core.Player
{
    [RequireComponent(typeof(FirstPersonController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FirstPersonController firstPersonController;

        private bool isLeaningLeft = false;
        private bool isLeaningRight = false;
        private bool wasLeanLeftPressed = false;
        private bool wasLeanRightPressed = false;

        void OnMove(InputValue value) => firstPersonController.moveInput = value.Get<Vector2>();
        void OnLook(InputValue value) => firstPersonController.lookInput = value.Get<Vector2>();
        void OnSprint(InputValue value) => firstPersonController.sprintInput = value.isPressed;
        void OnJump(InputValue value) { if (value.isPressed) firstPersonController.TryJump(); }
        void OnCrouch(InputValue value) { if (value.isPressed) firstPersonController.TryCrouch(); }
        void OnAttack(InputValue value) { if (value.isPressed) firstPersonController.TryAttack(); }
        void OnReload(InputValue value) { if (value.isPressed) firstPersonController.TryReload(); }

        void OnLeanLeft(InputValue value)
        {
            bool isPressed = value.isPressed;
            if (isPressed && !wasLeanLeftPressed)
            {
                if (isLeaningLeft) { isLeaningLeft = false; } // toggle off
                else
                {
                    isLeaningLeft = true;   // toggle on
                    isLeaningRight = false; // cancel other lean
                }
                UpdateLeanDirection();
            }

            wasLeanLeftPressed = isPressed;
        }

        void OnLeanRight(InputValue value)
        {
            bool isPressed = value.isPressed;
            if (isPressed && !wasLeanRightPressed)
            {
                if (isLeaningRight) { isLeaningRight = false; } // toggle off
                else
                {
                    isLeaningRight = true;   // toggle on
                    isLeaningLeft = false;   // cancel other lean
                }
                UpdateLeanDirection();
            }

            wasLeanRightPressed = isPressed;
        }


        private void UpdateLeanDirection()
        {
            if (isLeaningLeft && !isLeaningRight)
            {
                firstPersonController.TryLean(LeanHandler.LeanDirection.Left);
            }
            else if (isLeaningRight && !isLeaningLeft)
            {
                firstPersonController.TryLean(LeanHandler.LeanDirection.Right);
            }
            else
            {
                firstPersonController.TryLean(LeanHandler.LeanDirection.None);
            }
        }

        private void OnValidate()
        {
            if (firstPersonController == null)
                firstPersonController = GetComponent<FirstPersonController>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
