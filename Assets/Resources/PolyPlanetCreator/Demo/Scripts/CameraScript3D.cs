using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript3D : MonoBehaviour
{
    public float rotationSpeed = 50;

    private Camera m_cam;
    private Camera Cam
    {
        get
        {
            if (m_cam == null)
                m_cam = Camera.main;
            return m_cam;
        }
    }

    public float zoomSpeed = 30;
    public Vector2 minMaxZoom = new Vector2(-100, -20);

    void Update()
    {
        Rotation();
        Zoom();
    }

    private void Rotation()
    {
        transform.Rotate(new Vector2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal")) * rotationSpeed * Time.deltaTime);
    }

    private void Zoom()
    {
        if (Cam != null)
        {
            Cam.transform.localPosition = new Vector3(
                Cam.transform.localPosition.x,
                Cam.transform.localPosition.y,
                Mathf.Clamp(Cam.transform.localPosition.z + Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed * (2 - Mathf.Clamp01(-Mathf.Sign(Input.mouseScrollDelta.y))), minMaxZoom.x, minMaxZoom.y)
                );
        }
    }
}
