using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class SettingsMenuController : BaseMenuController
    {
        [Header("Submenus / Dependencies")]
        [SerializeField] private PauseMenuController pauseMenu;

        [Header("UI")]
        [SerializeField] private Button backButton;

        [Header("Audio Controls")]
        [SerializeField] private AudioSliderControl masterVolumeControl;
        [SerializeField] private AudioSliderControl musicVolumeControl;
        [SerializeField] private AudioSliderControl sfxVolumeControl;

        protected override void InitializeMenu()
        {
            base.InitializeMenu();
            if (backButton != null) SetupButton(backButton);
        }

        protected override void Start()
        {
            base.Start();
            ResetPauseState();
        }

        private void ResetPauseState()
        {
            if (pauseMenu != null)
            {
                pauseMenu.pauseMenuUI?.SetActive(false);
                PauseMenuController.isPaused = false;
                Time.timeScale = 1f;
            }
        }
    }
}
