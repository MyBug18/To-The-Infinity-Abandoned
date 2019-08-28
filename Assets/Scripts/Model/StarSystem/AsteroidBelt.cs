using System;

public class AsteroidBelt : CelestialBody
{
    public AsteroidBelt(Game game, StarOrbit starOrbit) : base(CelestialBodyType.AsteroidBelt, starOrbit, game)
    {
        name = starOrbit.center[0].name + " Belt";
    }
}