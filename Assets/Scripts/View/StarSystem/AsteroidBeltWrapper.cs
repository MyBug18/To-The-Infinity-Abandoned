using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBeltWrapper : CelestialBodyWrapper
{
    public AsteroidBelt asteroidBelt;

    [SerializeField]
    Transform[] asteroidPrefabs;

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
            int idx = GameDataHolder.r.Next(0, asteroidPrefabs.Length);
            Vector3 pos = new Vector3(orbitRadius * Mathf.Cos(i * Mathf.PI / asteroidBelt.asteroids.Count), orbitRadius * Mathf.Sin(i * Mathf.PI / asteroidBelt.asteroids.Count));
            Transform asteroid = Instantiate(asteroidPrefabs[idx], pos, Quaternion.identity, transform);
        }
    }
}
