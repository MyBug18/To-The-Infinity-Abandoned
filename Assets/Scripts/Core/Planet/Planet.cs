using System;

public abstract class Planet
{
    public string name { get; private set; }
    public int size { get; private set; }
    public PlanetType planetType { get; private set; }
}

public enum PlanetType
{
    Inhabitable,
    Barren,
    Gas
}
