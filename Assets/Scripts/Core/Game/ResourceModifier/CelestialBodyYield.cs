public class CelestialBodyYield : GlobalResourceModifiers
{
    public CelestialBody celestialBody;

    public CelestialBodyYield((GlobalResourceType type, float amount) v, CelestialBody celestialBody) : base(v, ModifierType.Yield)
    {
        this.celestialBody = celestialBody;
    }

    public override float GetModifier()
    {
        switch(value.type)
        {
            case GlobalResourceType.Engineering:
            case GlobalResourceType.Physics:
            case GlobalResourceType.Sociology:
                return celestialBody.game.researchStationModifier;
            default:
                return celestialBody.game.miningStationModifier;
        }
    }
}