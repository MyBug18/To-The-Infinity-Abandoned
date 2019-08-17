public class Resource
{
    public Game game;

    public Resource(Game game)
    {
        this.game = game;
        turnResource = new TurnResource(game);
    }

    public TurnResource turnResource;

    public float fuel = 0;
    public float mineral = 0;
    public float food = 0;
    public float money = 0;
    public float alloy = 0;
    public float physics = 0;
    public float sociology = 0;
    public float engineering = 0;

    public void ApplyTurnResource()
    {
        turnResource.ApplyAllModifiers();

        fuel += turnResource.turnFuel;
        mineral += turnResource.turnMineral;
        food += turnResource.turnFood;
        money += turnResource.turnMoney;
        alloy += turnResource.turnAlloy;
        physics += turnResource.turnPhysics;
        sociology += turnResource.turnSociology;
        engineering += turnResource.turnEngineering;
    }

    public override string ToString()
    {
        string _fuel = "Fuel: " + fuel + " (" + turnResource.turnFuel + ") ";
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
    Fuel,
    Mineral,
    Food,
    Money,
    Alloy,

    Physics,
    Sociology,
    Engineering
}