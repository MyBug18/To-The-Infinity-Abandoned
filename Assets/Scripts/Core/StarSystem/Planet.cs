using System;
using System.Collections.Generic;

public class Planet : CelestialBody
{
    public int size { get; private set; }
    public int nthOrbit { get; private set; }
    public PlanetType planetType { get; private set; }
    public float satelliteChance {  get
        {
            if (planetType == PlanetType.GasGiant) return 0.4f;
            else return 0.25f;
        }
    }

    public List<CelestialBody> satellites = new List<CelestialBody>();

    public Planet(string name, int size, PlanetType planetType, Game game, StarOrbit orbit, int nthOrbit) : base(CelestialBodyType.Planet, orbit, game) // for making special named planets, such as Mercury, Mars.
    {
        this.nthOrbit = nthOrbit;
        this.name = name;
        this.size = size;
        this.planetType = planetType;
    }

    public Planet(string name, Game game, StarOrbit starOrbit, int nthOrbit, bool isInhabitable = false) : base(CelestialBodyType.Planet, starOrbit, game) // for making general non-inhabitable planets.
    {
        this.nthOrbit = nthOrbit;
        this.name = name;
        size = GameManager.r.Next() % 15 + 11;

        if (isInhabitable)                       // sets planetType.
            planetType = PlanetType.Inhabitable;
        else
        {
            switch(GameManager.r.Next() % 6)
            {
                case 0:
                case 1:
                    planetType = PlanetType.GasGiant;
                    break;
                case 2:
                    planetType = PlanetType.Frozen;
                    break;
                case 3:
                    planetType = PlanetType.Broken;
                    break;
                case 4:
                    planetType = PlanetType.Molten;
                    break;
                default:
                    planetType = PlanetType.Barren;
                    break;
            }

            if (GameManager.r.Next() % 3 == 0) // 33% of having resources
            {
                switch(GameManager.r.Next() % 9)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        if (planetType == PlanetType.GasGiant)
                            yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, GameManager.r.Next() % 4 + 3), this));
                        else
                            yields.Add(new CelestialBodyYield((GlobalResourceType.Mineral, GameManager.r.Next() % 4 + 3), this));
                        break;
                    case 4:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Engineering, GameManager.r.Next() % 4 + 3), this));
                        break;
                    case 5:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Sociology, GameManager.r.Next() % 4 + 3), this));
                        break;
                    case 6:
                    case 7:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, GameManager.r.Next() % 4 + 3), this));
                        break;
                    case 8:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Alloy, GameManager.r.Next() % 3 + 1), this));
                        break;
                }
            }
        }
    }

    public void MakeSatellite()
    {
    }

    public override string ToString()
    {
        
        string result = "Size: " + size + ", Planet type: " + planetType + ", Orbiting " + starOrbit.center[0].name;
        result += "\nSatellites: ";
        foreach (var c in satellites)
        {
            result += c.name + ", ";
        }
        return "Planet name: " + name + "\n" + result;
    }

}

public enum PlanetType
{
    Inhabitable,
    Barren,
    GasGiant,
    Frozen,
    Molten,
    Broken
}
