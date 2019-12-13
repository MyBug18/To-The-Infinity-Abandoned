using System.Collections.Generic;

public abstract class CelestialBody
{
    public string name;
    public CelestialBodyType type;
    public Game game;
    public List<CelestialBodyYield> yields = new List<CelestialBodyYield>();
    public StarOrbit starOrbit;

    public int orbitalPeriod = GameDataHolder.random.Next(120, 720);
    public float rotationCycle = GameDataHolder.random.Next(30, 300) / 100;

    public float orbitRadius;
    public (float x, float y) positionComparedToOrbitHost;

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