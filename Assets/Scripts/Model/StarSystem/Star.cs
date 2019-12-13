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

        switch(GameDataHolder.random.Next() % 5)
        {
            case 0:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Physics, GameDataHolder.random.Next() % 3 + 2), this));
                break;
            case 1:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Engineering, GameDataHolder.random.Next() % 3 + 2), this));
                break;
            default:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Electricity, GameDataHolder.random.Next() % 4 + 3), this));
                break;
        }

        int typeDecider = GameDataHolder.random.Next() % 100;
        if (typeDecider < 3)
        {
            starType = StarType.O;
            orbitNum = GameDataHolder.random.Next() % 5 + 4;
            inhabitableChance = 0.1f;
            inhabitableRange = (4, 7);
        }
        else if (typeDecider < 8)
        {
            starType = StarType.B;
            orbitNum = GameDataHolder.random.Next() % 4 + 4;
            inhabitableChance = 0.1f;
            inhabitableRange = (5, 7);
        }
        else if (typeDecider < 15)
        {
            starType = StarType.A;
            orbitNum = GameDataHolder.random.Next() % 4 + 4;
            inhabitableChance = 0.15f;
            inhabitableRange = (5, 6);
        }
        else if (typeDecider < 24)
        {
            starType = StarType.F;
            orbitNum = GameDataHolder.random.Next() % 3 + 4;
            inhabitableChance = 0.2f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 35)
        {
            starType = StarType.G;
            orbitNum = GameDataHolder.random.Next() % 4 + 3;
            inhabitableChance = 0.2f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 49)
        {
            starType = StarType.K;
            orbitNum = GameDataHolder.random.Next() % 3 + 3;
            inhabitableChance = 0.2f;
            inhabitableRange = (4, 5);
        }
        else if (typeDecider < 69)
        {
            starType = StarType.M;
            orbitNum = GameDataHolder.random.Next() % 4 + 2;
            inhabitableChance = 0.2f;
            inhabitableRange = (3, 5);
        }
        else if (typeDecider < 84)
        {
            starType = StarType.RedGiant;
            orbitNum = GameDataHolder.random.Next() % 5 + 5;
            inhabitableChance = 0.15f;
            inhabitableRange = (5, 8);
        }
        else if (typeDecider < 89)
        {
            starType = StarType.WhiteDwarf;
            orbitNum = GameDataHolder.random.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 92)
        {
            starType = StarType.NeutronStar;
            orbitNum = GameDataHolder.random.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 95)
        {
            starType = StarType.Pulsar;
            orbitNum = GameDataHolder.random.Next() % 3 + 2;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
        else if (typeDecider < 100)
        {
            starType = StarType.BlackHole;
            orbitNum = GameDataHolder.random.Next() % 3 + 1;
            inhabitableChance = 0;
            inhabitableRange = (0, 0);
        }
    }

    public override string ToString()
    {
        string b = base.ToString();
        return b + "Star type: " + starType + "\n";
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