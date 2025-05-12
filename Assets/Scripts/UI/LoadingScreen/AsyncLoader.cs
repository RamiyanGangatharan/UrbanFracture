using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UrbanFracture.UI.LoadingScreen
{
    /// <summary>
    /// This is the enum that keeps track of scene indices based on the Unity build sequence
    /// </summary>
    public enum SceneEnum
    {
        MAIN_MENU = 0,
        GAME = 1,
        SETTINGS = 2,
        CREDITS = 3
    }

    public class AsyncManager : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameObject sceneFrom; // Scene you are coming from
        [SerializeField] private GameObject loadingScreen; // Loading Screen UI
        [SerializeField] private SliderController sliderController;

        [Header("UI Elements")]
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Scene Selection")]
        [SerializeField] private SceneEnum selectedScene;

        /// <summary>
        /// Called via Button OnClick — uses selectedScene dropdown value
        /// </summary>
        public void LoadSelectedScene() { LoadLevel(selectedScene); }

        /// <summary>
        /// Starts loading a scene from enum
        /// </summary>
        public void LoadLevel(SceneEnum levelToLoad)
        {
            if (sceneFrom != null) sceneFrom.SetActive(false);
            if (loadingScreen != null) loadingScreen.SetActive(true);

            StartCoroutine(LoadLevelAsync(levelToLoad.ToString()));
        }

        /// <summary>
        /// Asynchronously loads a scene and updates UI
        /// </summary>
        private IEnumerator LoadLevelAsync(string levelToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
            loadOperation.allowSceneActivation = false;

            float minLoadTime = 2f; // Minimum time to show loading screen
            float elapsedTime = 0f;
            float progress = 0f;

            while (!loadOperation.isDone)
            {
                // True progress until 0.9
                float targetProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);

                // Smoothly interpolate displayed progress toward actual
                progress = Mathf.MoveTowards(progress, targetProgress, Time.deltaTime);

                // Update UI
                if (loadingSlider != null) loadingSlider.value = progress;
                if (sliderController != null) sliderController.SliderChange(progress);
                if (loadingText != null) loadingText.text = $"{progress * 100f:0}%";

                elapsedTime += Time.deltaTime;

                // If the scene is basically done loading and the timer has run out
                if (loadOperation.progress >= 0.9f && elapsedTime >= minLoadTime)
                {
                    // Animate progress to 100% before switching scenes
                    while (progress < 1f)
                    {
                        progress = Mathf.MoveTowards(progress, 1f, Time.deltaTime);
                        if (loadingSlider != null) loadingSlider.value = progress;
                        if (sliderController != null) sliderController.SliderChange(progress);
                        if (loadingText != null) loadingText.text = $"{progress * 100f:0}%";
                        yield return null;
                    }

                    // Switch scenes
                    loadOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }



        /// <summary>
        /// Resets loading UI after the scene is loaded
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (loadingScreen != null) loadingScreen.SetActive(false);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Loads a scene from a string name
        /// </summary>
        public void LoadLevelByName(string sceneName)
        {
            if (System.Enum.TryParse(sceneName, out SceneEnum scene)) { LoadLevel(scene); }
        }
    }
}
