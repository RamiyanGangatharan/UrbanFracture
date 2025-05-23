using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UrbanFracture.UI.LoadingScreen
{
    /// <summary>
    /// This is the enum that keeps track of scene indices based on the Unity build sequence.
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

        public Canvas canvasToEnable;
        public Canvas canvasToDisable;

        [Header("UI Elements")]
        [SerializeField] private Slider loadingSlider;

        [Header("Scene Selection")]
        [SerializeField] private SceneEnum selectedScene;

        [Header("Background Image Configuration")]
        [SerializeField] private RawImage backgroundImage;
        [SerializeField] private Texture2D[] backgroundTextures; // Use Texture2D instead of Sprite
        [SerializeField] private float backgroundSwitchInterval = 2f;
        [SerializeField] private float crossfadeDuration = 1f;

        /// <summary>
        /// Called via Button OnClick — uses selectedScene dropdown value to load the scene.
        /// </summary>
        public void LoadSelectedScene() { LoadLevel(selectedScene); }

        /// <summary>
        /// Starts loading a scene from the given enum value.
        /// Displays the loading screen and begins the loading process.
        /// </summary>
        /// <param name="levelToLoad">The scene to load represented by the SceneEnum.</param>
        public void LoadLevel(SceneEnum levelToLoad)
        {
            if (sceneFrom != null) sceneFrom.SetActive(false);
            if (loadingScreen != null) loadingScreen.SetActive(true);

            StartCoroutine(LoadLevelAsync(levelToLoad.ToString()));
            StartCoroutine(BackgroundSlideshow());
        }

        /// <summary>
        /// Starts a slideshow of background images while the loading screen is active.
        /// Loops through the images with a specified interval.
        /// </summary>
        /// <returns>An IEnumerator for Coroutine.</returns>
        private IEnumerator BackgroundSlideshow()
        {
            sceneFrom.SetActive(false);

            int index = 0;
            while (loadingScreen.activeSelf)
            {
                if (backgroundTextures.Length == 0 || backgroundImage == null) yield break;

                Texture2D nextTexture = backgroundTextures[index];
                StartCoroutine(CrossfadeToTexture(nextTexture));
                index = (index + 1) % backgroundTextures.Length;

                yield return new WaitForSeconds(backgroundSwitchInterval);
            }
        }

        /// <summary>
        /// Smoothly crossfades between two background images.
        /// </summary>
        /// <param name="newTexture">The new texture to fade to.</param>
        /// <returns>An IEnumerator for Coroutine.</returns>
        private IEnumerator CrossfadeToTexture(Texture2D newTexture)
        {
            sceneFrom.SetActive(false);

            float t = 0f;
            Color color = backgroundImage.color;

            while (t < 1f)
            {
                t += Time.deltaTime / crossfadeDuration;
                backgroundImage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, t));
                yield return null;
            }

            backgroundImage.texture = newTexture;

            t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / crossfadeDuration;
                backgroundImage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, 1f, t));
                yield return null;
            }
        }

        /// <summary>
        /// Asynchronously loads a scene and updates the loading UI.
        /// The scene loading progress is tracked and displayed until fully loaded.
        /// </summary>
        /// <param name="levelToLoad">The scene name to load as a string.</param>
        /// <returns>An IEnumerator for Coroutine.</returns>
        private IEnumerator LoadLevelAsync(string levelToLoad)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
            loadOperation.allowSceneActivation = false;

            float minLoadTime = 10f;
            float elapsedTime = 0f;
            float progress = 0f;
            float targetProgress = 0f;

            // While the scene is loading
            while (!loadOperation.isDone)
            {
                // If scene is not yet finished loading, update progress based on loadOperation.progress
                if (loadOperation.progress < 0.9f)
                {
                    targetProgress = Mathf.Lerp(progress, loadOperation.progress / 0.9f, Time.deltaTime * 1.5f);
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                    targetProgress = Mathf.Lerp(progress, 1f, Mathf.Min(elapsedTime / minLoadTime, 1f));
                }

                // Update progress and UI
                progress = targetProgress;
                if (loadingSlider != null) loadingSlider.value = progress;
                if (sliderController != null) sliderController.SliderChange(progress);

                // If we reach 100% progress and the loading time has been met, we activate the scene
                if (progress >= 1f && elapsedTime >= minLoadTime)
                {
                    yield return new WaitForSeconds(0.5f);
                    loadOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }



        /// <summary>
        /// Resets the loading UI after the scene is loaded and deactivates the loading screen.
        /// This method is called when the scene has finished loading.
        /// </summary>
        /// <param name="scene">The scene that has just loaded.</param>
        /// <param name="mode">The mode in which the scene was loaded (e.g., Single, Additive).</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (loadingScreen != null) loadingScreen.SetActive(false);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Loads a scene by name.
        /// This method tries to parse the scene name into a SceneEnum and then calls LoadLevel with the enum value.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadLevelByName(string sceneName)
        {
            if (System.Enum.TryParse(sceneName, out SceneEnum scene)) { LoadLevel(scene); }
        }

        /// <summary>
        /// Helper function to switch between menu's
        /// </summary>
        public void SwitchCanvas()
        {
            canvasToDisable.gameObject.SetActive(false);
            canvasToEnable.gameObject.SetActive(true);
        }

        /// <summary>
        /// Helper function to disable a canvas on the fly 
        /// </summary>
        public void disableCanvas()
        {
            if (canvasToDisable != null) canvasToDisable.gameObject.SetActive(false);
        }    
    }
}
