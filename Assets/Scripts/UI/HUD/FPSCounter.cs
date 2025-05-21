using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UrbanFracture.UI.HUD
{
    /// <summary>
    /// Displays the average frames per second (FPS) over a specified sample size. 
    /// This class caches FPS values and computes an average over a given number of 
    /// frames to provide a smoothed FPS counter. The FPS can be displayed using either 
    /// smooth or unscaled delta time. Adapted from: https://gist.github.com/st4rdog/80057b406bfd00f44c8ec8796a071a13
    /// </summary>
    public class FPSCounter : MonoBehaviour
    {
        public enum DeltaTimeType { Smooth, Unscaled }

        [Header("UI Reference")]
        [SerializeField] private TMP_Text FPSText;

        [Header("Settings")]
        [SerializeField] private DeltaTimeType deltaType = DeltaTimeType.Smooth;

        [Tooltip("Maximum number cached as string.")]
        [SerializeField] private int cacheSize = 300;

        [Tooltip("How many frames to average over.")]
        [SerializeField] private int averageSampleSize = 30;

        private readonly Dictionary<int, string> cachedStrings = new();
        private int[] frameSamples;
        private int averageIndex;
        private int currentAverage;

        /// <summary>
        /// Initializes the FPSCounter by setting up the cache and frame sample array.
        /// </summary>
        private void Awake()
        {
            for (int i = 0; i < cacheSize; i++) { cachedStrings[i] = i.ToString(); }
            frameSamples = new int[averageSampleSize];
        }

        /// <summary>
        /// Called every frame to sample the frame rate, calculate the average FPS, and update the display.
        /// </summary>
        private void Update()
        {
            SampleFrameRate();
            CalculateAverage();
            UpdateDisplay();
        }

        /// <summary>
        /// Samples the frame rate using either smooth or unscaled delta time.
        /// Stores the FPS in the frame sample array.
        /// </summary>
        private void SampleFrameRate()
        {
            float deltaTime = deltaType switch
            {
                DeltaTimeType.Smooth => Time.smoothDeltaTime,
                DeltaTimeType.Unscaled => Time.unscaledDeltaTime,
                _ => Time.unscaledDeltaTime
            };

            int fps = Mathf.RoundToInt(1f / Mathf.Max(deltaTime, 0.0001f));
            frameSamples[averageIndex] = fps;
        }

        /// <summary>
        /// Calculates the average FPS from the frame samples.
        /// </summary>
        private void CalculateAverage()
        {
            float total = 0;

            for (int i = 0; i < frameSamples.Length; i++) { total += frameSamples[i]; }

            currentAverage = Mathf.RoundToInt(total / frameSamples.Length);
            averageIndex = (averageIndex + 1) % frameSamples.Length;
        }

        /// <summary>
        /// Updates the UI display with the current average FPS.
        /// </summary>
        private void UpdateDisplay()
        {
            if (currentAverage < 0) { FPSText.text = "< 0"; }
            else if (currentAverage < cacheSize) { FPSText.text = cachedStrings[currentAverage]; }
            else { FPSText.text = $"> {cacheSize}"; }
        }
    }
}
