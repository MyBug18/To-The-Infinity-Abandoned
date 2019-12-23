public abstract class Building : POPWorkingPlace
{
    protected Building(Planet_Inhabitable planet, float fuelUpkeep) : base(WorkingPlaceType.Building, planet)
    {
        baseUpkeep.resourceType = GlobalResourceType.Electricity;
        baseUpkeep.amount = fuelUpkeep;
    }
    public BuildingType buildingType { get; protected set; }
}

public enum BuildingType
{
    ColonizationCenter,
    PlanetaryCapital,
    AlloySmelter,
    AlloyFoundry,
    HouseOfWisdom,
    ResearchLab,
    PlanetaryMarket,
    GovernmentHall,
    PoliceStation,
    ControlTower,
    PlanetaryCommHub,
    FleetTrainingCenter,
    CommandCenter,
    MineralExtractor,
    BioPlant
}
