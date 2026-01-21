using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    // LateUpdate: for jitter-free camera facing
    void LateUpdate()
    {
        transform.forward = mainCam.transform.forward;
    }
}