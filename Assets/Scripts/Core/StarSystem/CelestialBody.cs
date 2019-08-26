using System.Collections.Generic;

public abstract class CelestialBody
{
    public string name;
    public CelestialBodyType type;
    public Game game;
    public List<CelestialBodyYield> yields = new List<CelestialBodyYield>();
    public StarOrbit starOrbit;

    protected CelestialBody(CelestialBodyType type, StarOrbit starOrbit, Game game)
    {
        this.starOrbit = starOrbit;
        this.type = type;
        this.game = game;
    }

    public override string ToString()
    {
        return type + " name: " + name + ", ";
    }
}

public enum CelestialBodyType
{
    Star,
    Planet,
    Asteroid,
    AsteroidBelt
}