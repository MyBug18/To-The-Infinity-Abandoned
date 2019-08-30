using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOrbitWrapper : MonoBehaviour
{
    public StarOrbit starOrbit;

    [SerializeField]
    Transform objectInOrbitPrefab;
    [SerializeField]
    Transform starPrefab, planetPrefab, asteroidPrefab, asteroidBeltPrefab;

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
        for (int i = 0; i < starOrbit.orbits.Count; i++)
        {
            CelestialBody body = starOrbit.orbits[i];
            Transform objectInOrbit = Instantiate(objectInOrbitPrefab, transform);
            objectInOrbit.GetComponent<ObjectInOrbit>().orbitLine.radius = body.orbitRadius;

            Transform parent = objectInOrbit.GetComponent<ObjectInOrbit>().orbitRotator;

            switch(body.type)
            {
                case CelestialBodyType.Planet:
                    Transform planet = Instantiate(planetPrefab, parent);
                    planet.GetComponent<PlanetWrapper>().body = body;
                    planet.localPosition = new Vector3(body.positionComparedToOrbitHost.x, body.positionComparedToOrbitHost.y);
                    planet.name = body.name;
                    break;

            }
        }
    }
}
