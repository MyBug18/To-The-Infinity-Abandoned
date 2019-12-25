using System;
public class TurnResource
{
    public Planet_Inhabitable planet;

    public TurnResource(Planet_Inhabitable planet)
    {
        this.planet = planet;
    }

    public float turnElectricity { get; private set; }
    public float turnMineral { get; private set; }
    public float turnFood { get; private set; }
    public float turnMoney { get; private set; }
    public float turnAlloy { get; private set; }
    public float turnPhysics { get; private set; }
    public float turnSociology { get; private set; }
    public float turnEngineering { get; private set; }

    public void ApplyAllChanges() // should be called with _IncreaseOneMonth().
    {
        turnElectricity = 0;
        turnMineral = 0;
        turnFood = 0;
        turnMoney = 0;
        turnAlloy = 0;
        turnPhysics = 0;
        turnSociology = 0;
        turnEngineering = 0;
        foreach (var grc in planet.planetBaseUpkeeps)
        {
            _ApplyOneChange(grc);
        }

        foreach (var grc in planet.planetJobUpkeeps)
        {
            _ApplyOneChange(grc);
        }

        foreach (var grc in planet.planetJobYields)
        {
            _ApplyOneChange(grc);
        }

        turnFood -= planet.pops.Count * planet.popFoodUpkeepRate;
    }

    private void _ApplyOneChange(GlobalResourceChanges grc)
    {
        var (_type, _amount) = grc.value;

        float amount;

        if (grc.changeType == ChangeType.Upkeep)
            amount = -_amount;
        else
            amount = _amount;

        switch (_type)
        {
            case GlobalResourceType.Alloy:
                turnAlloy += amount;
                break;
            case GlobalResourceType.Engineering:
                turnEngineering += amount;
                break;
            case GlobalResourceType.Food:
                turnFood += amount;
                break;
            case GlobalResourceType.Electricity:
                turnElectricity += amount;
                break;
            case GlobalResourceType.Mineral:
                UnityEngine.Debug.Log(turnMineral + " " + amount);
                turnMineral += amount;
                break;
            case GlobalResourceType.Money:
                turnMoney += amount;
                break;
            case GlobalResourceType.Physics:
                turnPhysics += amount;
                break;
            case GlobalResourceType.Sociology:
                turnSociology += amount;
                break;
            default:
                throw new InvalidOperationException("Undefined GlobalResourceType detected!");
        }

        if (planet.planetaryResources.isLackOfElectricity) turnMineral /= 2;
        if (planet.planetaryResources.isLackOfMineral) turnAlloy /= 4;
    }
}
