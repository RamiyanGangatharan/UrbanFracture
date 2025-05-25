using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    /// <summary>
    /// Controls a UI slider that adjusts a specific type of audio volume
    /// (Master, Music, or SFX) via the <see cref="AudioManager"/>.
    /// </summary>
    public class AudioSliderControl : MonoBehaviour
    {
        /// <summary>
        /// Enum representing the type of volume this slider controls.
        /// </summary>
        public enum VolumeType { Master, Music, SFX }

        [Header("UI")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI valueText;

        [Header("Settings")]
        [SerializeField] private VolumeType volumeType;

        private void Start()
        {
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnSliderValueChanged);
                LoadInitialValue();
            }
        }

        /// <summary>
        /// Loads the initial volume value from <see cref="AudioManager"/>
        /// and sets it on the slider without triggering the value change event.
        /// </summary>
        private void LoadInitialValue()
        {
            float volume = volumeType switch
            {
                VolumeType.Master => AudioManager.Instance.GetMasterVolume(),
                VolumeType.Music => AudioManager.Instance.GetMusicVolume(),
                VolumeType.SFX => AudioManager.Instance.GetSFXVolume(),
                _ => 1f
            };

            slider.SetValueWithoutNotify(volume);
            UpdateValueText(volume);
        }

        /// <summary>
        /// Called when the slider value is changed.
        /// Updates the corresponding volume in <see cref="AudioManager"/> and updates the UI text.
        /// </summary>
        /// <param name="value">New slider value between 0 and 1.</param>
        private void OnSliderValueChanged(float value)
        {
            switch (volumeType)
            {
                case VolumeType.Master: AudioManager.Instance.SetMasterVolume(value); break;
                case VolumeType.Music: AudioManager.Instance.SetMusicVolume(value); break;
                case VolumeType.SFX: AudioManager.Instance.SetSFXVolume(value); break;
            }

            UpdateValueText(value);
        }

        private void UpdateValueText(float value) { if (valueText != null) { valueText.text = $"{(value * 100f):F0}%"; } }
    }
}
