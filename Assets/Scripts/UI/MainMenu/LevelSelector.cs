using UnityEngine;
using UnityEngine.UI;
using UrbanFracture.UI.MainMenu;

namespace UrbanFracture.UI.MainMenu
{
    public class LevelSelector : MainMenuController
    {
        [SerializeField] private Button FactoryButton;

        protected override void InitializeMenu()
        {
            base.InitializeMenu();
            SetupButton(FactoryButton);
        }
    }
}
