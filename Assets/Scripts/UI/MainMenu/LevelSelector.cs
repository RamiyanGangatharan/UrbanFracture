using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class LevelSelector : BaseMenuController
    {
        [SerializeField] private Button factoryButton;
        [SerializeField] private Button blankButton_1;
        [SerializeField] private Button blankButton_2;
        [SerializeField] private Button backButton;

        protected override void InitializeMenu()
        {
            SetupButton(factoryButton);
            SetupButton(blankButton_1);
            SetupButton(blankButton_2);
            SetupButton(backButton);
        }
    }
}


