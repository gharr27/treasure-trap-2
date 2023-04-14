using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public float zoomSpeed = 1.0f;
    public float minZoom = 10.0f;
    public float maxZoom = 60.0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // zoom in and out with mouse scroll wheel
        if (scroll != 0.0f)
        {
            float newZoom = cam.fieldOfView - scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }
}
