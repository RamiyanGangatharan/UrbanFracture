using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Controls the main menu functionality, including button references and click audio setup.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button creditsButton;

        /// <summary>
        /// Initializes the main menu by attaching audio components to buttons.
        /// </summary>
        private void Start()
        {
            AttachAudio(playButton);
            AttachAudio(settingsButton);
            AttachAudio(quitButton);
            AttachAudio(creditsButton);
        }

        /// <summary>
        /// Attaches a UIButtonAudio component to the provided button if not already present.
        /// </summary>
        /// <param name="button">The button to attach the audio behavior to.</param>
        private void AttachAudio(Button button)
        {
            if (button == null) return;

            if (button.GetComponent<UrbanFracture.UI.Components.UIButtonAudio>() == null)
            {
                button.gameObject.AddComponent<UrbanFracture.UI.Components.UIButtonAudio>();
            }
        }
    }
}
