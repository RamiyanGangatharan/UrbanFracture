using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Controls the main menu functionality, including button references and audio feedback.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button creditsButton;

        [Header("Audio")]
        [SerializeField] private AudioSource hoverSound;
        [SerializeField] private AudioSource clickSound;

        protected virtual void InitializeMenu()
        {
            SetupButton(playButton);
            SetupButton(settingsButton);
            SetupButton(quitButton);
            SetupButton(creditsButton);
        }

        private void Start()
        {
            InitializeMenu();
        }

        /// <summary>
        /// Attaches audio behavior to the specified button.
        /// </summary>
        /// <param name="button">Button to set up.</param>
        protected void SetupButton(Button button)
        {
            if (button == null) return;

            button.onClick.AddListener(() => PlayClickSound()); // Add click sound

            // Add hover sound via EventTrigger
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
