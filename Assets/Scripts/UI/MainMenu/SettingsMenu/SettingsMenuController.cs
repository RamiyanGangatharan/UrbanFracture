using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class SettingsMenuController : BaseMenuController
    {
        [SerializeField] private Button backButton;

        protected override void InitializeMenu()
        {
            base.InitializeMenu();
            if (backButton != null) SetupButton(backButton);
        }

        protected override void Start() { base.Start(); }
    }
}
