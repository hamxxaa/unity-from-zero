using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef")]
    [SerializeField] Transform target;

    [Header("Ayarlar")]
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}