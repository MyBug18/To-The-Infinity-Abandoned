using System;

public class Asteroid : CelestialBody
{
    private Random _r = new Random();

    public Asteroid(Game game, StarOrbit starOrbit, (float x, float y) position) : base(CelestialBodyType.Asteroid, starOrbit, game)
    {
        name = _GenerateAsteroidName();
        switch(_r.Next(0, 4))
        {
            case 0:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Alloy, _r.Next(1, 3)), this));
                break;
            default:
                yields.Add(new CelestialBodyYield((GlobalResourceType.Mineral, _r.Next(3, 7)), this));
                break;
        }

        positionComparedToOrbitHost = position;
    }

    private string _GenerateAsteroidName()
    {
        return "" + _GetRandomChar() + _GetRandomChar() + "-" + _r.Next(0, 1000);
    }

    private char _GetRandomChar()
    {
        return (char)('A' + _r.Next(0, 26));
    }
}