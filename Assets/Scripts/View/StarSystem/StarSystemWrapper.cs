using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemWrapper : MonoBehaviour
{
    public StarSystem system;
    public Transform starOrbitPrefab;
    public GameManager gameManager;

    public List<StarOrbitWrapper> starOrbits = new List<StarOrbitWrapper>();

    private float _orbitDistance;

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
        foreach (var orb in system.orbits)
        {
            Transform orbit = Instantiate(starOrbitPrefab, transform);
            orbit.GetComponent<StarOrbitWrapper>().starOrbit = orb;
            orbit.GetComponent<StarOrbitWrapper>().gameManager = gameManager;
            orbit.localPosition = new Vector3(orb.positionInStarSystem.x, orb.positionInStarSystem.y);
            starOrbits.Add(orbit.GetComponent<StarOrbitWrapper>());
        }
    }
}
