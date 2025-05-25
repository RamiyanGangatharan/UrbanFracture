using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UrbanFracture.UI.MainMenu
{
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

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FindAudioSourcesInScene();
            ApplyVolumes();
        }

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
                if (source != null && !musicAudioSources.Contains(source))
                    musicAudioSources.Add(source);
            }

            // Find all SFX audio sources by tag "SFX"
            var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
            foreach (var obj in sfxObjects)
            {
                var source = obj.GetComponent<AudioSource>();
                if (source != null && !sfxAudioSources.Contains(source))
                    sfxAudioSources.Add(source);
            }
        }

        private void ApplyVolumes()
        {
            // Apply master volume first - scales music and sfx volumes
            float effectiveMusicVolume = musicVolume * masterVolume;
            float effectiveSFXVolume = sfxVolume * masterVolume;

            // Set volume on all music audio sources
            foreach (var source in musicAudioSources)
            {
                source.volume = effectiveMusicVolume;
            }

            // Set volume on all SFX audio sources
            foreach (var source in sfxAudioSources)
            {
                source.volume = effectiveSFXVolume;
            }
        }

        // Public setters to update volumes and save prefs

        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
            ApplyVolumes();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            ApplyVolumes();
        }

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

        private void LoadVolumes()
        {
            masterVolume = GetMasterVolume();
            musicVolume = GetMusicVolume();
            sfxVolume = GetSFXVolume();
        }
    }
}
