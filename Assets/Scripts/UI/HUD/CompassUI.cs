using System.Collections.Generic;
using UnityEngine;

public class CompassBar : MonoBehaviour
{
    public RectTransform compassBarTransform;
    public Transform cameraTransform;

    [System.Serializable]
    public class CompassMarker
    {
        public string direction;
        public RectTransform marker;
        public Vector3 worldDirection;
    }

    public List<CompassMarker> markers;

    void Update()
    {
        foreach (var marker in markers)
        {
            UpdateMarker(marker);
        }
    }

    private void UpdateMarker(CompassMarker markerData)
    {
        Vector3 flatForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 flatMarkerDir = markerData.worldDirection.normalized;

        float angle = Vector3.SignedAngle(flatForward, flatMarkerDir, Vector3.up);
        float normalizedAngle = angle / 180f; // -1 (left) to 1 (right)

        float halfWidth = compassBarTransform.rect.width / 2f;
        float xPos = normalizedAngle * halfWidth;

        markerData.marker.anchoredPosition = new Vector2(xPos, 0);
    }
}
