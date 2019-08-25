using System;

public class Star : CelestialBody
{
    public StarType starType;
    public int orbitNum;
    public float inhabitableChance;
    public (int min, int max) inhabitableRange;

    public Star(Game game, StarOrbit starOrbit, string name) : base(CelestialBodyType.Star, starOrbit, game)
    {
        this.name = name;
        this.starOrbit = starOrbit;

        Random r = new Random();

        switch(r.Next() % 5)
        {
            case 0:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Physics, r.Next() % 3 + 2), this));
                break;
            case 1:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Engineering, r.Next() % 3 + 2), this));
                break;
            default:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, r.Next() % 4 + 3), this));
                break;
        }

        int typeDecider = r.Next() % 100;
        if (typeDecider < 3)
        {
            starType = StarType.O;
            orbitNum = r.Next() % 5 + 4;
            inhabitableChance = 0.02f;
            inhabitableRange = (4, 7);
        }
        else if (typeDecider < 8)
        {
            starType = StarType.B;
            orbitNum = r.Next() % 4 + 4;
            inhabitableChance = 0.02f;
            inhabitableRange = (5, 7);
        }
        else if (typeDecider < 15)
        {
            starType = StarType.A;
            orbitNum = r.Next() % 4 + 4;
            inhabitableChance = 0.05f;
            inhabitableRange = (5, 6);
        }
        else if (typeDecider < 24)
        {
            starType = StarType.F;
            orbitNum = r.Next() % 3 + 4;
            inhabitableChance = 0.1f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 35)
        {
            starType = StarType.G;
            orbitNum = r.Next() % 4 + 3;
            inhabitableChance = 0.1f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 49)
        {
            starType = StarType.K;
            orbitNum = r.Next() % 3 + 3;
            inhabitableChance = 0.1f;
            inhabitableRange = (4, 5);
        }
        else if (typeDecider < 69)
        {
            starType = StarType.M;
            orbitNum = r.Next() % 4 + 2;
            inhabitableChance = 0.1f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 84)
        {
            starType = StarType.RedGiant;
            orbitNum = r.Next() % 5 + 5;
            inhabitableChance = 0.05f;
            inhabitableRange = (5, 8);
        }
        else if (typeDecider < 89)
        {
            starType = StarType.WhiteDwarf;
            orbitNum = r.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 92)
        {
            starType = StarType.NeutronStar;
            orbitNum = r.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 95)
        {
            starType = StarType.Pulsar;
            orbitNum = r.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 100)
        {
            starType = StarType.BlackHole;
            orbitNum = r.Next() % 3 + 1;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
    }

    public override string ToString()
    {
        string b = base.ToString();
        return b + "Star type: " + starType;
    }
}

public enum StarType

{
    O,
    B,
    A,
    F,
    G,
    K,
    M,
    RedGiant,
    WhiteDwarf,
    NeutronStar,
    Pulsar,
    BlackHole
}