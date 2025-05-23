using UnityEngine;
using UnityEngine.UI;
using UrbanFracture.UI.MainMenu;

public class LevelSelector : BaseMenuController
{
    [SerializeField] private Button factoryButton;

    protected override void InitializeMenu()
    {
        SetupButton(factoryButton);
    }
}
