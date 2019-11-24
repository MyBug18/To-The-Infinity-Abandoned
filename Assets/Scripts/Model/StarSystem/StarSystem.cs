using System;
using System.Collections.Generic;

// A single star system can contain many stars, up to 3.
// Each star can act as a host of an orbit, or many stars can form an orbit together.
// To prevent the confusion of the term "Star System", which may refer to an orbit formed by a star, or the cluster of (up to 3) orbits,
// I'll call the orbit formed by star (or stars) "Star Orbit".
// So, in a single star system, there may exist up to 3 star orbits.

public class StarSystem
{
    public Game game;
    public List<StarOrbit> orbits = new List<StarOrbit>();
    public string name;
    private int _starNum = 0;

    public int numberOfOrbits;

    public (int, int) coordinate; // The identifier of star system

    public StarSystem(Game game)
    {
        Random r = new Random();
        this.game = game;
        _SetCoordinate();
        name = game.GetStarSystemName();

        int shapeDecider = r.Next() % 20;

        float secondOrbitAngle = (float)(Math.PI / 2 * GameDataHolder.r.NextDouble());
        float thirdOrbitAngle = (float)(Math.PI / 2 * GameDataHolder.r.NextDouble() + Math.PI);
        
        if (shapeDecider < 8)
        {
            numberOfOrbits = 1;
            orbits.Add(new StarOrbit(1, this));
        }
        else if (shapeDecider < 13)
        {
            numberOfOrbits = 2;
            orbits.Add(new StarOrbit(1, this));
            orbits.Add(new StarOrbit(1, this));
        }
        else if (shapeDecider < 16)
        {
            numberOfOrbits = 3;
            orbits.Add(new StarOrbit(1, this));
            orbits.Add(new StarOrbit(1, this));
            orbits.Add(new StarOrbit(1, this));
        }
        else if (shapeDecider < 18)
        {
            numberOfOrbits = 1;
            orbits.Add(new StarOrbit(2, this));
        }
        else if (shapeDecider < 19)
        {
            numberOfOrbits = 2;
            orbits.Add(new StarOrbit(2, this));
            orbits.Add(new StarOrbit(1, this));
        }
        else if (shapeDecider < 20)
        {
            numberOfOrbits = 1;
            orbits.Add(new StarOrbit(3, this));
        }

        orbits.Sort((a, b) => b.orbits.Count.CompareTo(a.orbits.Count));

        float _orbitDistance;

        orbits[0].positionInStarSystem = (0, 0);
        if (orbits.Count > 1)
        {
            _orbitDistance = orbits[0].orbits[orbits[0].orbits.Count - 1].orbitRadius + orbits[1].orbits[orbits[1].orbits.Count - 1].orbitRadius + 3;
            orbits[1].positionInStarSystem = (_orbitDistance * (float)Math.Cos(secondOrbitAngle), _orbitDistance * (float)Math.Sin(secondOrbitAngle));

            if (orbits.Count > 2)
            {
                _orbitDistance = orbits[0].orbits[orbits[0].orbits.Count - 1].orbitRadius + orbits[2].orbits[orbits[2].orbits.Count - 1].orbitRadius + 3;
                orbits[2].positionInStarSystem = (_orbitDistance * (float)Math.Cos(thirdOrbitAngle), _orbitDistance * (float)Math.Sin(thirdOrbitAngle));
            }
        }
    }

    public string GetStarName()
    {
        _starNum++;
        switch (_starNum)
        {
            case 1:
                return name;
            case 2:
                return name + " B";
            default:
                return name + " C";
        }
    }

    private void _SetCoordinate()
    {
        int c = game.systems.Count;
        int x = c / 10;
        int y = c % 10;
        coordinate = (x * 70, y * 70);
    }

    public override string ToString()
    {
        var s = "System name: " + name + "\nOrbits:\n";
        foreach (var o in orbits)
            s += o;
        return s;
    }
}