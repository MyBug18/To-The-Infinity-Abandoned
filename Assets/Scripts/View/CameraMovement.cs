using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int _width;
    private int _height;

    public GameObject test;

    public int speed = 1;
    public int Boundary = 50;

    public float rotLimit = 70;

    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 5.0f;
    float rotY = 0.0f;

    private Vector3 cameraVector => transform.position - mainCamera.localPosition;

    [SerializeField]
    private Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > _width - Boundary)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0); // move on +X axis
        }

        if (Input.mousePosition.x < 0 + Boundary)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0); // move on -X axis
        }

        if (Input.mousePosition.y > _height - Boundary)
        {
            transform.Translate(0, speed * Time.deltaTime, 0); // move on +Z axis
        }

        if (Input.mousePosition.y < 0 + Boundary)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0); // move on -Z axis
        }

        if (Input.mouseScrollDelta.y > 0 && cameraVector.magnitude > 10)
        {
            mainCamera.localPosition += new Vector3(0, 0, 10);
        }

        if (Input.mouseScrollDelta.y < 0 && cameraVector.magnitude < 100)
            mainCamera.localPosition += new Vector3(0, 0, -10);

        if (Input.GetMouseButton(1))
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);

            if (-rotY < -rotLimit) rotY = rotLimit;
            if (-rotY > rotLimit) rotY = -rotLimit;

            if (rotX < 360 - rotLimit && rotX > 180) rotX = 360 - rotLimit;
            if (rotX > rotLimit && rotX < 180) rotX = rotLimit;

            Vector3 toRot = new Vector3(-rotY, rotX, 0.0f);

            transform.localEulerAngles = toRot;
        }

        //Debug
        if (Input.GetMouseButton(0))
        {
            Vector3 angleOfCamera = transform.GetChild(0).eulerAngles;
            Vector3 positionOfCamera = transform.GetChild(0).position;
            float distanceOfCameraFromOrigin = positionOfCamera.magnitude;

            Vector3 positionOfCameraDiffByAngle = new Vector3(
                distanceOfCameraFromOrigin * Mathf.Sin(angleOfCamera.x),
                distanceOfCameraFromOrigin * Mathf.Sin(angleOfCamera.y),
                distanceOfCameraFromOrigin * Mathf.Sin(angleOfCamera.z)
                );

            Vector3 positionOfMouseOnScreen = Input.mousePosition;
            positionOfMouseOnScreen.z = 1;

            Vector3 positionOfMouseInWorld = Camera.main.ScreenToWorldPoint(positionOfMouseOnScreen);

            Vector3 positionOfMouseOnPlane = positionOfCameraDiffByAngle + (positionOfMouseInWorld - positionOfCamera);

            test.transform.position = positionOfMouseOnPlane;

            Debug.Log(positionOfMouseOnPlane);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }
    }
}