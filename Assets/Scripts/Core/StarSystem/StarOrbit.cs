using System;
using System.Collections.Generic;

public class StarOrbit
{
    public StarSystem system;
    public List<Star> center = new List<Star>();
    public List<CelestialBody> orbits;

    public int orbitNum;
    public float inhabitableChance;
    public (int min, int max) inhabitableRange;

    public StarOrbit(int starNum, StarSystem system)
    {
        this.system = system;
        orbits = new List<CelestialBody>();

        for (int i = 0; i < starNum; i++)
            center.Add(new Star(system.game, this, system.GetStarName()));

        orbitNum = center[0].orbitNum;
        inhabitableRange.min = center[0].inhabitableRange.min;
        inhabitableRange.max = center[0].inhabitableRange.max;
        inhabitableChance = center[0].inhabitableChance;

        foreach(var s in center)
        {
            if (s.orbitNum < orbitNum) orbitNum = s.orbitNum;
            if (s.inhabitableRange.min < inhabitableRange.min) inhabitableRange.min = s.inhabitableRange.min;
            if (s.inhabitableRange.max < inhabitableRange.max) inhabitableRange.max = s.inhabitableRange.max; // sets the StarOrbit's inhabitable range to it's center star's minimum range.
            if (s.inhabitableChance > inhabitableChance) inhabitableChance = s.inhabitableChance;
        }

        if (center.Count > 1)
        {
            orbitNum /= 2;
            inhabitableRange.min /= 2;
            inhabitableRange.max /= 2;
        }

        for (int i = 0; i < orbitNum; i++) // make planets, asteroids
        {
            _MakeOrbits(i);
        }
    }

    public void _MakeOrbits(int nth)
    {
        UnityEngine.Debug.Log(nth + ", " + inhabitableRange + ", " + inhabitableChance);
        if (GameManager.r.Next() % 100 < inhabitableChance * 100 && inhabitableRange.min <= nth && inhabitableRange.max >= nth)
        {
            orbits.Add(new Planet_Inhabitable(_GetPlanetName(nth), system.game, this));
        }
        else if ((nth == orbitNum || nth == orbitNum / 2) && orbitNum > 4)
        {
            orbits.Add(new AsteroidBelt(system.game, this));
        }
        else
        {
            var p = new Planet(_GetPlanetName(nth), system.game, this);

            orbits.Add(p);
        }
    }

    private string _GetPlanetName(int orbitNum)
    {
        return center[0].name + " " + _toRoman(orbitNum);
    }

    public override string ToString()
    {
        string s = "Stars: ";
        foreach (var star in center)
            s += star + " / ";

        s += "\n";

        string o = "Planets:\n";
        foreach (var orb in orbits)
            o += orb + "\n";

        return s + o;
    }

    private string _toRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value between 1 and 3999");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + _toRoman(number - 1000);
        if (number >= 900) return "CM" + _toRoman(number - 900);
        if (number >= 500) return "D" + _toRoman(number - 500);
        if (number >= 400) return "CD" + _toRoman(number - 400);
        if (number >= 100) return "C" + _toRoman(number - 100);
        if (number >= 90) return "XC" + _toRoman(number - 90);
        if (number >= 50) return "L" + _toRoman(number - 50);
        if (number >= 40) return "XL" + _toRoman(number - 40);
        if (number >= 10) return "X" + _toRoman(number - 10);
        if (number >= 9) return "IX" + _toRoman(number - 9);
        if (number >= 5) return "V" + _toRoman(number - 5);
        if (number >= 4) return "IV" + _toRoman(number - 4);
        if (number >= 1) return "I" + _toRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
    }
}
