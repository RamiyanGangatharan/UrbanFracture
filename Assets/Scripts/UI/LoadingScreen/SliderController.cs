using UnityEngine;

/// <summary>
/// Controls the behavior of a loading slider, allowing progress updates
/// based on a normalized float value (0 to 1).
/// </summary>
public class SliderController : MonoBehaviour
{
    [SerializeField] private float maxSliderAmount = 100.0f;

    /// <summary>
    /// Updates the slider value based on a normalized input value.
    /// The value is scaled by <see cref="maxSliderAmount"/>.
    /// </summary>
    /// <param name="value">Normalized progress value (0 to 1).</param>
    public void SliderChange(float value) { float localValue = value * maxSliderAmount; }
}
