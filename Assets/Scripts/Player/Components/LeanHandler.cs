using UnityEngine;

public class LeanHandler : MonoBehaviour
{
    [Header("Lean Settings")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float leanAngle = 15f;
    [SerializeField] private float leanSpeed = 10f;

    private float targetZRotation = 0f;

    /// <summary>
    /// presets for leaning directions.
    /// </summary>
    public enum LeanDirection { None, Left, Right }

    /// <summary>
    /// This function sets the lean direction of the camera pivot.
    /// </summary>
    /// <param name="direction"></param>
    public void SetLean(LeanDirection direction)
    {
        switch (direction)
        {
            case LeanDirection.Left: targetZRotation = leanAngle; break;
            case LeanDirection.Right: targetZRotation = -leanAngle; break;
            case LeanDirection.None: targetZRotation = 0f; break;
        }
    }

    private void Update() { ApplyLean(); }

    /// <summary>
    /// This function applies the lean effect to the camera pivot based on the target rotation.
    /// </summary>
    private void ApplyLean()
    {
        if (cameraPivot == null) return;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZRotation);
        cameraPivot.localRotation = Quaternion.Lerp(
            cameraPivot.localRotation,
            targetRotation,
            Time.deltaTime * leanSpeed
        );
    }
}
