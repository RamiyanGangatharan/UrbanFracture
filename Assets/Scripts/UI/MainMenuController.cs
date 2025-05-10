using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UrbanFracture
{
    /// <summary>
    /// Controls the main menu functionality, including button interactions and hover sound effects.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        public Button playButton;
        public Button settingsButton;
        public Button quitButton;
        public Button creditsButton;

        public AudioSource hoverButtonSound;
        public AudioSource clickButtonSound;

        /// <summary>
        /// Initializes the main menu by adding sound triggers to all buttons.
        /// </summary>
        void Start()
        {
            AddHoverSound(playButton);
            AddClickSound(playButton);

            AddHoverSound(settingsButton);
            AddClickSound(settingsButton);

            AddHoverSound(quitButton);
            AddClickSound(quitButton);

            AddHoverSound(creditsButton);
            AddClickSound(creditsButton);
        }

        /// <summary>
        /// Adds a hover sound effect trigger to the specified button.
        /// </summary>
        /// <param name="button">The button to which the hover sound will be added.</param>
        void AddHoverSound(Button button)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) { trigger = button.gameObject.AddComponent<EventTrigger>(); }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { PlayHoverSound(); });

            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// Adds a click sound effect trigger to the specified button.
        /// </summary>
        /// <param name="button">The button to which the click sound will be added.</param>
        void AddClickSound(Button button) { button.onClick.AddListener(() => PlayClickSound()); }

        /// <summary>
        /// Plays the hover sound effect.
        /// </summary>
        public void PlayHoverSound() { hoverButtonSound.Play(); }

        /// <summary>
        /// Plays the click sound effect.
        /// </summary>
        public void PlayClickSound() { clickButtonSound.Play(); }
    }
}