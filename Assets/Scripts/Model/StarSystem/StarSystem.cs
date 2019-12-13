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
    public StarOrbit orbit { get; private set; }
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

        float secondOrbitAngle = (float)(Math.PI / 2 * GameDataHolder.random.NextDouble());
        float thirdOrbitAngle = (float)(Math.PI / 2 * GameDataHolder.random.NextDouble() + Math.PI);
        
        if (shapeDecider < 8)
        {
            numberOfOrbits = 1;
            orbit = new StarOrbit(1, this);
        }
        else if (shapeDecider < 18)
        {
            numberOfOrbits = 1;
            orbit = new StarOrbit(2, this);
        }
        else if (shapeDecider < 20)
        {
            numberOfOrbits = 1;
            orbit = new StarOrbit(3, this);
        }

        orbit.positionInStarSystem = (0, 0);
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
        var s = "System name: " + name + "\nOrbits:\n" + orbit;
        return s;
    }
}