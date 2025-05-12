using TMPro;
using UnityEngine;

namespace UrbanFracture.UI.LoadingScreen
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField] private float maxSliderAmount = 100.0f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SliderChange(float value)
        {
            float localValue = value * maxSliderAmount;
        }
    }
}
