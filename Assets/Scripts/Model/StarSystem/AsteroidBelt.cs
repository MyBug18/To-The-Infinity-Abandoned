using System.Collections.Generic;

public class AsteroidBelt : CelestialBody
{
    public List<Asteroid> asteroids = new List<Asteroid>();

    public AsteroidBelt(Game game, StarOrbit starOrbit) : base(CelestialBodyType.AsteroidBelt, starOrbit, game)
    {
        name = starOrbit.center[0].name + " Belt";

        for (int i = 0; i < GameDataHolder.r.Next(4, 9); i++)
            asteroids.Add(new Asteroid(game, starOrbit));
    }
}