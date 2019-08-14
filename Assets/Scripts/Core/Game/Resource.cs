using System;
using System.Collections;
using System.Collections.Generic;

public class Resource
{
    public float fuel { get; private set; } = 0;
    public float mineral { get; private set; } = 0;
    public float food { get; private set; } = 0;
    public float money { get; private set; } = 0;
    public float alloy { get; private set; } = 0;
    public float physics { get; private set; } = 0;
    public float sociology { get; private set; } = 0;
    public float engineering { get; private set; } = 0;

    public float turnFuel { get; private set; } = 0;
    public float turnMineral { get; private set; } = 0;
    public float turnFood { get; private set; } = 0;
    public float turnMoney { get; private set; } = 0;
    public float turnAlloy { get; private set; } = 0;
    public float turnPhysics { get; private set; } = 0;
    public float turnSociology { get; private set; } = 0;
    public float turnEngineering { get; private set; } = 0;

    public void ModifyResource((GlobalResourceType, float) value)
    {
        GlobalResourceType type = value.Item1;
        float amount = value.Item2;
        switch(type)
        {
            case GlobalResourceType.Fuel:
                fuel += amount;
                break;
            case GlobalResourceType.Mineral:
                mineral += amount;
                break;
            case GlobalResourceType.Food:
                food += amount;
                break;
            case GlobalResourceType.Money:
                money += amount;
                break;
            case GlobalResourceType.Alloy:
                alloy += amount;
                break;
            case GlobalResourceType.Physics:
                physics += amount;
                break;
            case GlobalResourceType.Sociology:
                sociology += amount;
                break;
            case GlobalResourceType.Engineering:
                engineering += amount;
                break;
            default:
                throw new InvalidOperationException("ERROR: Invalid GlobalResourceType Used: " + type);

        }
    }
    public void ModifyResourceTurn((GlobalResourceType, float) value)
    {
        GlobalResourceType type = value.Item1;
        float amount = value.Item2;
        switch (type)
        {
            case GlobalResourceType.Fuel:
                turnFuel += amount;
                break;
            case GlobalResourceType.Mineral:
                turnMineral += amount;
                break;
            case GlobalResourceType.Food:
                turnFood += amount;
                break;
            case GlobalResourceType.Money:
                turnMoney += amount;
                break;
            case GlobalResourceType.Alloy:
                turnAlloy += amount;
                break;
            case GlobalResourceType.Physics:
                turnPhysics += amount;
                break;
            case GlobalResourceType.Sociology:
                turnSociology += amount;
                break;
            case GlobalResourceType.Engineering:
                turnEngineering += amount;
                break;
            default:
                throw new InvalidOperationException("ERROR: Invalid GlobalResourceType Used: " + type);

        }
    }

    public void ApplyTurnRecources() // Should be called with _IncreaseOneMonth().
    {
        fuel += turnFuel;
        mineral += turnMineral;
        food += turnFood;
        money += turnMoney;
        alloy += turnAlloy;
        physics += turnPhysics;
        sociology += turnSociology;
        engineering += turnEngineering;
    }

}

public enum GlobalResourceType // Only global resources.
{
    Fuel,
    Mineral,
    Food,
    Money,
    Alloy,

    Physics,
    Sociology,
    Engineering
}