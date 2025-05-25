using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UrbanFracture.UI.LoadingScreen;

namespace UrbanFracture.UI.MainMenu
{
    public class PauseMenuController : BaseMenuController
    {
        public Button resumeButton;
        public Button MainMenuButton;
        public Button QuitButton;

        public GameObject pauseMenuUI;
        public static bool isPaused;

        public AudioSource pauseMusic;

        private InputAction pauseAction;


        protected override void InitializeMenu()
        {
            base.InitializeMenu();

            SetupButton(resumeButton);
            SetupButton(MainMenuButton);
            SetupButton(QuitButton);

            resumeButton.onClick.AddListener(ResumeGame);
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            QuitButton.onClick.AddListener(QuitGame);
        }

        protected override void Start()
        {
            base.Start();
            pauseMenuUI.SetActive(false);
        }

        private void OnEnable()
        {
            pauseAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
            pauseAction.performed += ctx => TogglePause();
            pauseAction.Enable();
        }

        private void OnDisable()
        {
            pauseAction.Disable();
            pauseAction.performed -= ctx => TogglePause();
        }

        private void TogglePause()
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        public void PauseGame()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pauseMusic.Play();
        }

        public void ResumeGame()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseMusic.Pause();
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene((int)SceneEnum.MAIN_MENU);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }
}
