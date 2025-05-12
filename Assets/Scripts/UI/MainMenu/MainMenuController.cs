using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Controls the main menu functionality (button references and click actions).
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button creditsButton;

        private void Start()
        {
            // Attach sound handler to each button
            AttachAudio(playButton);
            AttachAudio(settingsButton);
            AttachAudio(quitButton);
            AttachAudio(creditsButton);
        }

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
