using System;

public class Resource
{
    public Planet_Inhabitable planet;

    public Resource(Planet_Inhabitable planet)
    {
        this.planet = planet;
        turnResource = new TurnResource(planet);
    }

    public TurnResource turnResource;
    public float electricity = 0;
    public float mineral = 0;
    public float food = 0;
    public float money = 0;
    public float alloy = 0;
    public float physics = 0;
    public float sociology = 0;
    public float engineering = 0;

    public bool isLackOfElectricity => electricity == 0 && turnResource.turnElectricity < 0;
    public bool isLackOfMineral => mineral == 0 && turnResource.turnMineral < 0;
    public bool isLackOfFood => food == 0 && turnResource.turnFood < 0;
    public bool isLackOfMoney => money == 0 && turnResource.turnMoney < 0;
    public bool isLackOfAlloy => alloy == 0 && turnResource.turnAlloy < 0;

    public void ApplyTurnResource()
    {
        turnResource.ApplyAllModifiers();

        electricity += turnResource.turnElectricity;
        if (electricity < 0) electricity = 0;
        mineral += turnResource.turnMineral;
        if (mineral < 0) mineral = 0;
        food += turnResource.turnFood;
        if (food < 0) food = 0;
        money += turnResource.turnMoney;
        if (money < 0) money = 0;
        alloy += turnResource.turnAlloy;
        if (alloy < 0) alloy = 0;
        physics += turnResource.turnPhysics;
        sociology += turnResource.turnSociology;
        engineering += turnResource.turnEngineering;
    }

    public override string ToString()
    {
        string _fuel = "Fuel: " + electricity + " (" + turnResource.turnElectricity + ") ";
        string _mineral = "Mineral: " + mineral + " (" + turnResource.turnMineral + ") ";
        string _food = "Food: " + food + " (" + turnResource.turnFood + ") ";
        string _money = "Money: " + money + " (" + turnResource.turnMoney + ") ";
        string _alloy = "Alloy: " + alloy + " (" + turnResource.turnAlloy + ") ";
        string _physics = "Physics: " + physics + " (" + turnResource.turnPhysics + ") ";
        string _sociology = "Sociology: " + sociology + " (" + turnResource.turnSociology + ") ";
        string _engineering = "Engineering: " + engineering + " (" + turnResource.turnEngineering + ") ";

        return _fuel + _mineral + _food + _money + _alloy + _physics + _sociology + _engineering;
    }
}

public enum GlobalResourceType // Only global resources.
{
    Electricity,
    Mineral,
    Food,
    Money,
    Alloy,

    Physics,
    Sociology,
    Engineering
}
