using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBeltWrapper : CelestialBodyWrapper
{
    public AsteroidBelt asteroidBelt;
    
    public Transform asteroidPrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        asteroidBelt = (AsteroidBelt)body;

        _InstantiateAsteroids();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    private void _InstantiateAsteroids()
    {
        for (int i = 0; i < asteroidBelt.asteroids.Count; i++)
        {
            Asteroid ast = asteroidBelt.asteroids[i];

            Transform asteroid = Instantiate(asteroidPrefab, transform);
            asteroid.GetComponent<AsteroidWrapper>().body = ast;
            asteroid.localPosition = new Vector3(ast.positionComparedToOrbitHost.x, ast.positionComparedToOrbitHost.y);
        }
    }
}
