using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UrbanFracture.UI.MainMenu
{
    public class AudioSliderControl : MonoBehaviour
    {
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

        private void OnSliderValueChanged(float value)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    AudioManager.Instance.SetMasterVolume(value);
                    break;
                case VolumeType.Music:
                    AudioManager.Instance.SetMusicVolume(value);
                    break;
                case VolumeType.SFX:
                    AudioManager.Instance.SetSFXVolume(value);
                    break;
            }

            UpdateValueText(value);
        }

        private void UpdateValueText(float value)
        {
            if (valueText != null)
            {
                valueText.text = $"{(value * 100f):F0}%";
            }
        }
    }
}
