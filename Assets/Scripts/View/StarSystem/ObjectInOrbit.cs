using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInOrbit : MonoBehaviour
{
    public Transform orbitRotator;
    public DrawOrbitLine orbitLine;

    public CelestialBody holdingBody;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.objectInOrbitRotationEvents += new GameManager.ObjectInOrbitRotationEvent(_RotateThroughOrbit);
    }


    private void _RotateThroughOrbit()
    {
        float angleByOneDay = 360f / holdingBody.orbitalPeriod;
        orbitRotator.transform.Rotate(0, 0, angleByOneDay * Time.deltaTime);
    }
}
