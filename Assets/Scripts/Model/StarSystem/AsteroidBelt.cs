using System;
using System.Collections.Generic;

public class AsteroidBelt : CelestialBody
{
    public List<Asteroid> asteroids = new List<Asteroid>();

    public AsteroidBelt(Game game, StarOrbit starOrbit, int nthOrbit) : base(CelestialBodyType.AsteroidBelt, starOrbit, game)
    {
        name = starOrbit.center[0].name + " Belt";

        orbitRadius = (nthOrbit + 2.5f) * 3.5f;

        int asteroidNum = GameDataHolder.r.Next(7, 9);

        for (int i = 0; i < asteroidNum; i++)
        {
            var angle = 2 * Math.PI / asteroidNum;
            asteroids.Add(new Asteroid(game, starOrbit, ((float)(orbitRadius * Math.Cos(angle * i)), (float)(orbitRadius * Math.Sin(angle * i)))));
        }
        positionComparedToOrbitHost = (0, 0);
    }
}