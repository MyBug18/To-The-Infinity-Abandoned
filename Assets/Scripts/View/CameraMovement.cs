using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int _width;
    private int _height;
    public int speed = 1;
    public int Boundary = 50;

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

        if (Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 1)
            Camera.main.orthographicSize--;

        if (Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 7)
            Camera.main.orthographicSize++;
    }
}
