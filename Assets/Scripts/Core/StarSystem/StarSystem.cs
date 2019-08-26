using System;
using System.Collections.Generic;

public class StarSystem
{
    public Game game;
    public List<StarOrbit> orbits = new List<StarOrbit>();
    public string name;
    private int _starNum = 0;

    public int numberOfOrbits;

    public StarSystem(Game game)
    {
        Random r = new Random();
        this.game = game;

        name = game.GetStarSystemName();

        int shapeDecider = r.Next() % 20;
        
        if (shapeDecider < 13)
        {
            numberOfOrbits = 1;
            orbits.Add(new StarOrbit(1, this));
        }
        else if (shapeDecider < 15)
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

    public override string ToString()
    {
        var s = "System name: " + name + "\nOrbits:\n";
        foreach (var o in orbits)
            s += o;
        return s;
    }
}