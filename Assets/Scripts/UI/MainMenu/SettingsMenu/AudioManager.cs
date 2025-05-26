using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Manages audio volume settings for master, music, and sound effects (SFX).
    /// Automatically applies volume settings to tagged audio sources in each scene.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Volume Keys for PlayerPrefs")]
        private const string MasterVolumeKey = "Audio_MasterVolume";
        private const string MusicVolumeKey = "Audio_MusicVolume";
        private const string SFXVolumeKey = "Audio_SFXVolume";

        private float masterVolume = 1f;
        private float musicVolume = 1f;
        private float sfxVolume = 1f;

        private List<AudioSource> musicAudioSources = new List<AudioSource>();
        private List<AudioSource> sfxAudioSources = new List<AudioSource>();

        /// <summary>
        /// Initializes the singleton instance and loads volume settings.
        /// Subscribes to scene load events to manage audio sources.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadVolumes();

            // Subscribe to scene loaded event to refresh audio sources
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Find audio sources in the current scene as well
            FindAudioSourcesInScene();
        }

        private void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }

        /// <summary>
        /// Called when a new scene is loaded. Refreshes audio sources and re-applies volume settings.
        /// </summary>
        /// <param name="scene">The scene that was loaded.</param>
        /// <param name="mode">The mode in which the scene was loaded.</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FindAudioSourcesInScene();
            ApplyVolumes();
        }

        /// <summary>
        /// Finds all audio sources in the scene tagged with "MUSIC" or "SFX"
        /// and populates the respective lists.
        /// </summary>
        private void FindAudioSourcesInScene()
        {
            // Clear old lists before populating
            musicAudioSources.Clear();
            sfxAudioSources.Clear();

            // Find all music audio sources by tag "MUSIC"
            var musicObjects = GameObject.FindGameObjectsWithTag("MUSIC");
            foreach (var obj in musicObjects)
            {
                var source = obj.GetComponent<AudioSource>();
                if (source != null && !musicAudioSources.Contains(source)) { musicAudioSources.Add(source); }
            }

            // Find all SFX audio sources by tag "SFX"
            var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
            foreach (var obj in sfxObjects)
            {
                var source = obj.GetComponent<AudioSource>();
                if (source != null && !sfxAudioSources.Contains(source)) { sfxAudioSources.Add(source); }
            }
        }

        /// <summary>
        /// Applies volume levels to all music and SFX audio sources
        /// based on current settings and master volume.
        /// </summary>
        private void ApplyVolumes()
        {
            // Apply master volume first - scales music and sfx volumes
            float effectiveMusicVolume = musicVolume * masterVolume;
            float effectiveSFXVolume = sfxVolume * masterVolume;

            // Set volume on all music and SFX audio sources
            foreach (var source in musicAudioSources) { source.volume = effectiveMusicVolume; }
            foreach (var source in sfxAudioSources) { source.volume = effectiveSFXVolume; }
        }

        /// <summary>
        /// Sets the master volume level, saves it to PlayerPrefs, and applies changes.
        /// </summary>
        /// <param name="value">Volume value between 0 and 1.</param>
        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
            ApplyVolumes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            ApplyVolumes();
        }

        /// <summary>
        /// Sets the music volume level, saves it to PlayerPrefs, and applies changes.
        /// </summary>
        /// <param name="value">Volume value between 0 and 1.</param>
        public void SetSFXVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
            ApplyVolumes();
        }

        // Public getters to retrieve saved volumes

        public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        public float GetMusicVolume() => PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        public float GetSFXVolume() => PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        /// <summary>
        /// 
        /// </summary>
        private void LoadVolumes()
        {
            masterVolume = GetMasterVolume();
            musicVolume = GetMusicVolume();
            sfxVolume = GetSFXVolume();
        }
    }
}
