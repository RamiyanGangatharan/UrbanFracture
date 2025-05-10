using UnityEngine;
using TMPro;

namespace UrbanFracture
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sliderText = null;
        [SerializeField] private float maximumSliderAmount = 100.0f;

        public void SliderChange(float value)
        {
            float localValue = value * maximumSliderAmount;
            sliderText.text = localValue.ToString("0");
        }
    }
}