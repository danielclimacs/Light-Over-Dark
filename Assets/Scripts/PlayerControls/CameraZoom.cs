using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine; 

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float smoothTime = 8f;
    public float minZoom = 3f;   // Closest zoom in
    public float maxZoom = 10f;  // Farthest zoom out

    private CinemachineCamera vcam;
    private float targetZoom;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        targetZoom = vcam.Lens.OrthographicSize;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // Read mouse scroll wheel
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0f)
        {
            targetZoom -= Mathf.Sign(scroll) * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // Smoothly interpolate to the target zoom
        vcam.Lens.OrthographicSize = Mathf.Lerp(
            vcam.Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * smoothTime
        );
    }
}