using System;

public abstract class Planet
{
    public string name { get; private set; }
    public int size { get; private set; }
    public PlanetType planetType { get; private set; }

    public Planet(string name, int size, PlanetType planetType)
    {
        this.name = name;
        this.size = size;
        this.planetType = planetType;
    }

    public override string ToString()
    {
        return "Name: " + name + ", Size: " + size + ", Type: " + planetType + "\n";
    }

}

public enum PlanetType
{
    Inhabitable,
    Barren,
    Gas
}
