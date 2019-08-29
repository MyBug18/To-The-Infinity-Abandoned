using System;
using System.Collections.Generic;

public class Planet : CelestialBody
{
    public int size { get; private set; }
    public int nthOrbit { get; private set; }
    public PlanetType planetType { get; private set; }
    public float satelliteChance {  get
        {
            if (planetType == PlanetType.GasGiant) return 0.33f;
            else if (planetType == PlanetType.Broken) return 0.18f;
            else return 0.12f;
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
        size = GameDataHolder.r.Next() % 15 + 11;

        if (isInhabitable)                       // sets planetType.
            planetType = PlanetType.Inhabitable;
        else
        {
            switch(GameDataHolder.r.Next() % 6)
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

            if (GameDataHolder.r.Next() % 3 == 0) // 33% of having resources
            {
                switch(GameDataHolder.r.Next() % 9)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        if (planetType == PlanetType.GasGiant)
                            yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, GameDataHolder.r.Next() % 4 + 3), this));
                        else
                            yields.Add(new CelestialBodyYield((GlobalResourceType.Mineral, GameDataHolder.r.Next() % 4 + 3), this));
                        break;
                    case 4:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Engineering, GameDataHolder.r.Next() % 4 + 3), this));
                        break;
                    case 5:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Sociology, GameDataHolder.r.Next() % 4 + 3), this));
                        break;
                    case 6:
                    case 7:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, GameDataHolder.r.Next() % 4 + 3), this));
                        break;
                    case 8:
                        yields.Add(new CelestialBodyYield((GlobalResourceType.Alloy, GameDataHolder.r.Next() % 3 + 1), this));
                        break;
                }
            }
        }

        MakeSatellite();
    }

    public void MakeSatellite()
    {
        while(true)
        {
            if (satelliteChance * 100 > GameDataHolder.r.Next() % 100)
            {
                if (nthOrbit <= starOrbit.inhabitableRange.max && nthOrbit >= starOrbit.inhabitableRange.min && GameDataHolder.r.Next() % 100 < starOrbit.inhabitableChance * 100)
                {
                    satellites.Add(new Planet_Inhabitable(_GetSatelliteName(), game, starOrbit, satellites.Count));
                }
                else if (GameDataHolder.r.Next() % 10 < 2)
                {
                    satellites.Add(new Asteroid(game, starOrbit));
                }
                else
                {
                    PlanetType type;
                    switch(GameDataHolder.r.Next() % 9)
                    {
                        case 0:
                            type = PlanetType.Molten;
                            break;
                        case 1:
                        case 2:
                            type = PlanetType.Frozen;
                            break;
                        case 3:
                        case 4:
                            type = PlanetType.Broken;
                            break;
                        default:
                            type = PlanetType.Barren;
                            break;
                    }
                    satellites.Add(new Planet(_GetSatelliteName(), GameDataHolder.r.Next(5, size * 2 / 3), type, game, starOrbit, satellites.Count));
                }
            }
            else return;
        }
    }

    private string _GetSatelliteName()
    {
        return name + (char)('a' + satellites.Count);
    }

    public override string ToString()
    {        
        string result = "Size: " + size + ", Planet type: " + planetType + ", Orbiting " + starOrbit.center[0].name;
        if (satellites.Count > 0)
        {
            result += "\nSatellites: ";
            foreach (var c in satellites)
            {
                result += c.name + ", ";
            }
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
