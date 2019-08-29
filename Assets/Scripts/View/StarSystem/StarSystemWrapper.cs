using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemWrapper : MonoBehaviour
{
    public StarSystem system;
    public Transform starOrbitPrefab;

    public List<StarOrbitWrapper> starOrbits = new List<StarOrbitWrapper>();

    private int _orbitDistance;

    // Start is called before the first frame update
    void Start()
    {
        _InstantiateStarOrbits(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _InstantiateStarOrbits()
    {
        Transform firstOrbit = Instantiate(starOrbitPrefab, new Vector3(0, 0), Quaternion.identity, transform);
        firstOrbit.GetComponent<StarOrbitWrapper>().starOrbit = system.orbits[0];
        starOrbits.Add(firstOrbit.GetComponent<StarOrbitWrapper>()); // make first orbit.

        if (system.orbits.Count > 1) // if there is more orbit
        {
            _orbitDistance = system.orbits[0].orbits.Count + system.orbits[1].orbits.Count + 3;
            float _secondOrbitAngle = Random.Range(0, Mathf.PI / 2);
            Transform secondOrbit = Instantiate(starOrbitPrefab, new Vector3(_orbitDistance * Mathf.Cos(_secondOrbitAngle), _orbitDistance * Mathf.Sin(_secondOrbitAngle)), Quaternion.identity, transform);
            secondOrbit.GetComponent<StarOrbitWrapper>().starOrbit = system.orbits[1];
            starOrbits.Add(secondOrbit.GetComponent<StarOrbitWrapper>());

            if (system.orbits.Count > 2) // if there is more orbit
            {
                _orbitDistance = system.orbits[0].orbits.Count + system.orbits[2].orbits.Count + 3;
                float _thirdOrbitAngle = Random.Range(Mathf.PI, Mathf.PI * 3 / 2);
                Transform thirdOrbit = Instantiate(starOrbitPrefab, new Vector3(_orbitDistance * Mathf.Cos(_thirdOrbitAngle), _orbitDistance * Mathf.Sin(_thirdOrbitAngle)), Quaternion.identity, transform);
                thirdOrbit.GetComponent<StarOrbitWrapper>().starOrbit = system.orbits[2];
                starOrbits.Add(thirdOrbit.GetComponent<StarOrbitWrapper>());
            }
        }
    }
}
