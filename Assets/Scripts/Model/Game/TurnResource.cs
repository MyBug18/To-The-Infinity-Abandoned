using System;
public class TurnResource
{
    public Game game;

    public TurnResource(Game game)
    {
        this.game = game;
    }

    public float turnElectricity { get; private set; }
    public float turnMineral { get; private set; }
    public float turnFood { get; private set; }
    public float turnMoney { get; private set; }
    public float turnAlloy { get; private set; }
    public float turnPhysics { get; private set; }
    public float turnSociology { get; private set; }
    public float turnEngineering { get; private set; }

    public void ApplyAllModifiers() // should be called with _IncreaseOneMonth().
    {
        turnElectricity = 0;
        turnMineral = 0;
        turnFood = 0;
        turnMoney = 0;
        turnAlloy = 0;
        turnPhysics = 0;
        turnSociology = 0;
        turnEngineering = 0;


        foreach (var planet in game.colonizedPlanets)
        {
            foreach (var grm in planet.planetBaseUpkeeps)
            {
                _ApplyOneModifier(grm);
            }

            foreach (var grm in planet.planetJobUpkeeps)
            {
                _ApplyOneModifier(grm);
            }

            foreach (var grm in planet.planetJobYields)
            {
                _ApplyOneModifier(grm);
            }

            turnFood -= planet.pops.Count * game.popFoodUpkeepRate;
        }
    }

    private void _ApplyOneModifier(GlobalResourceChanges grm)
    {
        var (_type, _amount) = grm.value;
        UnityEngine.Debug.Log(grm.changeType + " " + _type.ToString() + " " + _amount);

        float amount;

        if (grm.changeType == ChangeType.Upkeep)
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

        if (game.globalResource.isLackOfElectricity) turnMineral /= 2;
        if (game.globalResource.isLackOfMineral) turnAlloy /= 4;
    }
}
