using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class BaseMenuController : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource hoverSound;
        [SerializeField] private AudioSource clickSound;

        protected virtual void InitializeMenu() { }

        protected virtual void Start() { InitializeMenu(); }

        /// <summary>
        /// Attaches audio behavior to the specified button.
        /// </summary>
        protected void SetupButton(Button button)
        {
            if (button == null) return;

            button.onClick.AddListener(() => PlayClickSound());

            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) { trigger = button.gameObject.AddComponent<EventTrigger>(); }

            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };

            entry.callback.AddListener(_ => PlayHoverSound());
            trigger.triggers.Add(entry);
        }

        private void PlayHoverSound() { if (hoverSound != null) hoverSound.Play(); }

        private void PlayClickSound() { if (clickSound != null) clickSound.Play(); }
    }
}