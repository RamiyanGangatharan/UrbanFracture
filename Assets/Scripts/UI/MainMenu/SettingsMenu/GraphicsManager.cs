using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Manages graphics-related UI settings such as resolution, quality level, texture quality,
    /// and anti-aliasing. Applies changes in real-time and persists them using PlayerPrefs.
    /// </summary>
    public class GraphicsManager : MonoBehaviour
    {
        [Header("Graphics Controls")]
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown qualityDropdown;
        public TMP_Dropdown antiAliasingDropdown;
        public TMP_Dropdown textureDropdown;

        private Resolution[] resolutions;
        private bool suppressCallbacks = false;

        private void Start()
        {
            resolutions = Screen.resolutions;

            PopulateResolutionOptions();
            PopulateQualityOptions();
            PopulateTextureOptions();
            PopulateAAOptions();

            AddDropdownListeners();
            LoadSettings(PlayerPrefs.GetInt("ResolutionPreference", GetCurrentResolutionIndex()));
        }

        /// <summary>
        /// Populates the resolution dropdown with unique resolution options.
        /// </summary>
        private void PopulateResolutionOptions()
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            HashSet<string> seen = new HashSet<string>();
            int currentIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;

                if (!seen.Add(option)) { continue; }

                options.Add(option);
                if
                (
                    resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height
                )
                {
                    currentIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentIndex;
            resolutionDropdown.RefreshShownValue();
        }

        /// <summary>
        /// Populates the quality dropdown with preset options.
        /// </summary>
        private void PopulateQualityOptions()
        {
            qualityDropdown.ClearOptions();
            List<string> options = new List<string>(QualitySettings.names);
            options.Add("Custom"); // index 6
            qualityDropdown.AddOptions(options);
            qualityDropdown.RefreshShownValue();
        }

        /// <summary>
        /// Populates the texture dropdown with preset options.
        /// </summary>
        private void PopulateTextureOptions()
        {
            textureDropdown.ClearOptions();
            textureDropdown.AddOptions(new List<string> { "Ultra", "High", "Medium", "Low" });
            textureDropdown.RefreshShownValue();
        }

        /// <summary>
        /// Populates the anti aliasing dropdown with preset options.
        /// </summary>
        private void PopulateAAOptions()
        {
            antiAliasingDropdown.ClearOptions();
            antiAliasingDropdown.AddOptions(new List<string> { "Off", "2x", "4x", "8x" });
        }


        /// <summary>
        /// Registers dropdown value change listeners to their corresponding handlers.
        /// </summary>
        private void AddDropdownListeners()
        {
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            qualityDropdown.onValueChanged.AddListener(SetQuality);
            textureDropdown.onValueChanged.AddListener(SetTextureQuality);
            antiAliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
        }

        /// <summary>
        /// Toggles fullscreen mode.
        /// </summary>
        /// <param name="isFullscreen">Whether the screen should be fullscreen.</param>
        public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

        /// <summary>
        /// Sets the screen resolution based on the selected dropdown index.
        /// </summary>
        /// <param name="resolutionIndex">Index of the selected resolution.</param>
        public void SetResolution(int resolutionIndex)
        {
            if (suppressCallbacks) { return; }
            resolutionIndex = Mathf.Clamp(resolutionIndex, 0, resolutions.Length - 1);
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, false);
        }

        /// <summary>
        /// Sets the global texture quality level.
        /// </summary>
        /// <param name="index">Dropdown index (mipmap level).</param>
        public void SetTextureQuality(int index)
        {
            if (suppressCallbacks) return;
            QualitySettings.globalTextureMipmapLimit = index;
            qualityDropdown.value = 6; // Custom
        }

        /// <summary>
        /// Sets anti-aliasing level.
        /// </summary>
        /// <param name="index">Dropdown index corresponding to 0x, 2x, 4x, or 8x.</param>
        public void SetAntiAliasing(int index)
        {
            if (suppressCallbacks) return;

            int aaLevel = index switch
            {
                0 => 0,
                1 => 2,
                2 => 4,
                3 => 8,
                _ => 0
            };

            QualitySettings.antiAliasing = aaLevel;
            qualityDropdown.value = 6; // Custom
        }

        /// <summary>
        /// Sets the overall quality level and updates dependent dropdowns.
        /// </summary>
        /// <param name="index">Quality level index (0 to 5) or 6 for custom.</param>
        public void SetQuality(int index)
        {
            if (suppressCallbacks) return;

            if (index != 6) QualitySettings.SetQualityLevel(index); // Only apply predefined levels

            suppressCallbacks = true;

            switch (index)
            {
                case 0: textureDropdown.value = 3; antiAliasingDropdown.value = 0; break;
                case 1: textureDropdown.value = 2; antiAliasingDropdown.value = 0; break;
                case 2: textureDropdown.value = 1; antiAliasingDropdown.value = 0; break;
                case 3: textureDropdown.value = 0; antiAliasingDropdown.value = 0; break;
                case 4: textureDropdown.value = 0; antiAliasingDropdown.value = 1; break;
                case 5: textureDropdown.value = 0; antiAliasingDropdown.value = 2; break;
            }

            qualityDropdown.value = index;
            textureDropdown.RefreshShownValue();
            antiAliasingDropdown.RefreshShownValue();
            suppressCallbacks = false;
        }

        /// <summary>
        /// Saves the current graphics settings to PlayerPrefs.
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
            PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
            PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
            PlayerPrefs.SetInt("AntiAliasingPreference", antiAliasingDropdown.value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads saved graphics settings from PlayerPrefs and applies them.
        /// </summary>
        /// <param name="resolutionIndex">Fallback resolution index if no saved value exists.</param>
        public void LoadSettings(int resolutionIndex)
        {
            suppressCallbacks = true;

            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference", 3);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference", resolutionIndex);
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference", 0);
            antiAliasingDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference", 1);

            qualityDropdown.RefreshShownValue();
            resolutionDropdown.RefreshShownValue();
            textureDropdown.RefreshShownValue();
            antiAliasingDropdown.RefreshShownValue();

            suppressCallbacks = false;
            ApplyAllSettings();
        }

        /// <summary>
        /// Applies all selected settings in the UI dropdowns to Unity's quality settings.
        /// </summary>
        private void ApplyAllSettings()
        {
            SetQuality(qualityDropdown.value);
            SetResolution(resolutionDropdown.value);
            SetTextureQuality(textureDropdown.value);
            SetAntiAliasing(antiAliasingDropdown.value);
        }

        /// <summary>
        /// Finds the index of the current screen resolution in the list of available resolutions.
        /// </summary>
        /// <returns>The index of the current resolution.</returns>
        private int GetCurrentResolutionIndex()
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (
                    resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height
                )
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
