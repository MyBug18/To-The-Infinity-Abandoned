using System;

public class Resource
{
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

        private void _ApplyOneModifier(GlobalResourceModifiers grm)
        {
            var (_type, _amount) = grm.value;

            float amount;

            if (grm.modifierType == ModifierType.Upkeep)
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

    public Game game;

    public Resource(Game game)
    {
        this.game = game;
        turnResource = new TurnResource(game);
    }

    public TurnResource turnResource;
    public float Electricity = 0;
    public float mineral = 0;
    public float food = 0;
    public float money = 0;
    public float alloy = 0;
    public float physics = 0;
    public float sociology = 0;
    public float engineering = 0;

    public bool isLackOfElectricity => Electricity == 0 && turnResource.turnElectricity < 0;
    public bool isLackOfMineral => mineral == 0 && turnResource.turnMineral < 0;
    public bool isLackOfFood => food == 0 && turnResource.turnFood < 0;
    public bool isLackOfMoney => money == 0 && turnResource.turnMoney < 0;
    public bool isLackOfAlloy => alloy == 0 && turnResource.turnAlloy < 0;

    public void ApplyTurnResource()
    {
        turnResource.ApplyAllModifiers();

        Electricity += turnResource.turnElectricity;
        if (Electricity < 0) Electricity = 0;
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
        string _fuel = "Fuel: " + Electricity + " (" + turnResource.turnElectricity + ") ";
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