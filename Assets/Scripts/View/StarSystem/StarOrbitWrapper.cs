using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOrbitWrapper : MonoBehaviour
{
    public StarOrbit starOrbit;

    [SerializeField]
    Transform orbitLinePrefab, parentOfAllCelestialBodies, parentOfAllOrbitLines;
    [SerializeField]
    Transform starPrefab, planetPrefab, asteroidPrefab, asteroidBeltPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _DrawOrbitLines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _DrawOrbitLines()
    {
        for (int i = 0; i < starOrbit.orbits.Count; i++)
        {
            int radius = i + 2;
            Transform orbitLine = Instantiate(orbitLinePrefab, transform.position, Quaternion.identity, parentOfAllOrbitLines);
            orbitLine.GetComponent<DrawOrbitLine>().radius = radius;
        }
    }
}
