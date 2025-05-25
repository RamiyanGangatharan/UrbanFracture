using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class MainMenuController : BaseMenuController
    {
        [Header("Main Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button creditsButton;

        protected override void InitializeMenu()
        {
            SetupButton(playButton);
            SetupButton(settingsButton);
            SetupButton(quitButton);
            SetupButton(creditsButton);
        }
    }

}
