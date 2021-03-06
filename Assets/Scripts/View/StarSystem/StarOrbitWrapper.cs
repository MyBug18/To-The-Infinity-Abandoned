﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOrbitWrapper : MonoBehaviour
{
    public StarOrbit starOrbit;

    [SerializeField]
    Transform objectInOrbitPrefab;
    [SerializeField]
    Transform starPrefab, planetPrefab, asteroidPrefab, asteroidBeltPrefab;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _InstantiateFirstOrbits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _InstantiateFirstOrbits()
    {
        foreach (var body in starOrbit.bodiesInOrbit)
        {
            Transform objectInOrbit = Instantiate(objectInOrbitPrefab, transform);
            objectInOrbit.GetComponent<ObjectInOrbit>().orbitLine.radius = body.orbitRadius;
            objectInOrbit.name = body.name + " Orbit";
            Transform parent = objectInOrbit.GetComponent<ObjectInOrbit>().orbitRotator;

            switch(body.type)
            {
                case CelestialBodyType.Planet:
                    Transform planet = Instantiate(planetPrefab, parent);
                    parent.parent.GetComponent<ObjectInOrbit>().holdingBody = body;
                    parent.parent.GetComponent<ObjectInOrbit>().gameManager = gameManager;
                    planet.GetComponent<PlanetWrapper>().body = body;
                    planet.localPosition = new Vector3(body.positionComparedToOrbitHost.x, body.positionComparedToOrbitHost.y);
                    planet.name = body.name;

                    Transform satellites = planet.GetComponent<PlanetWrapper>().satellites;
                
                    foreach(var sat in ((Planet)body).satellites)
                    {
                        Transform objectInOrbitSat = Instantiate(objectInOrbitPrefab, satellites);
                        objectInOrbitSat.GetComponent<ObjectInOrbit>().orbitLine.radius = sat.orbitRadius;

                        Transform parentSat = objectInOrbitSat.GetComponent<ObjectInOrbit>().orbitRotator;
                        parentSat.parent.GetComponent<ObjectInOrbit>().holdingBody = body;
                        parentSat.parent.GetComponent<ObjectInOrbit>().gameManager = gameManager;
                        Transform satellite = Instantiate(planetPrefab, parentSat);
                        satellite.GetComponent<PlanetWrapper>().body = sat;
                        satellite.localPosition = new Vector3(sat.positionComparedToOrbitHost.x, sat.positionComparedToOrbitHost.y);
                        satellite.name = sat.name;
                    }

                    break;
                case CelestialBodyType.AsteroidBelt:
                    Transform asteroidBelt = Instantiate(asteroidBeltPrefab, parent);
                    asteroidBelt.GetComponent<AsteroidBeltWrapper>().body = body;
                    parent.parent.GetComponent<ObjectInOrbit>().gameManager = gameManager;
                    parent.parent.GetComponent<ObjectInOrbit>().holdingBody = body;
                    asteroidBelt.name = body.name;
                    break;
            }
        }
    }
}
