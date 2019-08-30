using System;
using System.Collections.Generic;

public class AsteroidBelt : CelestialBody
{
    public List<Asteroid> asteroids = new List<Asteroid>();

    public AsteroidBelt(Game game, StarOrbit starOrbit, int nthOrbit) : base(CelestialBodyType.AsteroidBelt, starOrbit, game)
    {
        name = starOrbit.center[0].name + " Belt";

        orbitRadius = (nthOrbit + 2) * 1.5f;

        int asteroidNum = GameDataHolder.r.Next(4, 9);

        for (int i = 0; i < asteroidNum; i++)
        {
            asteroids.Add(new Asteroid(game, starOrbit, (orbitRadius * (float)Math.Cos(Math.PI * 2 * GameDataHolder.r.NextDouble()), orbitRadius * (float)Math.Sin(Math.PI * 2 * GameDataHolder.r.NextDouble()))));
        }
        positionComparedToOrbitHost = (0, 0);
    }
}