using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncLoader : MonoBehaviour
{
    [Header("MENU SCREENS")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("SLIDER")]
    [SerializeField] private Slider loadingSlider;

    [Header("Loading Speed")]
    [SerializeField] private float artificialLoadingSpeed = 0.3f; // Adjust this to control how slow it loads

    public void LoadLevelButton(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false; // Prevents auto-switch until we manually trigger

        float fakeProgress = 0f;

        while (fakeProgress < 1f)
        {
            // Move fakeProgress toward 1 artificially
            fakeProgress += Time.deltaTime * artificialLoadingSpeed;
            loadingSlider.value = Mathf.Clamp01(fakeProgress);

            yield return null;

            // Once real loading is done and our fake progress is 95%+, allow the scene to switch
            if (loadOperation.progress >= 0.9f && fakeProgress >= 0.95f)
            {
                break;
            }
        }

        // Ensure slider fills to 100%
        loadingSlider.value = 1f;

        // Optional: short pause before switching
        yield return new WaitForSeconds(0.5f);

        loadOperation.allowSceneActivation = true;
    }
}
