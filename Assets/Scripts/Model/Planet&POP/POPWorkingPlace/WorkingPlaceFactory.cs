using System;

public class WorkingPlaceFactory
{
    public Planet_Inhabitable planet;
    public Game game;

    public WorkingPlaceFactory(Planet_Inhabitable planet)
    {
        this.planet = planet;
    }

    public District GetDistrict(DistrictType type, Planet_Inhabitable planet)
    {
        switch(type)
        {
            case DistrictType.Electricity:
                return new ElectricityDistrict(planet);
            case DistrictType.Mineral:
                return new MineralDistrict(planet);
            case DistrictType.Food:
                return new FoodDistrict(planet);
            case DistrictType.House:
                return new HouseDistrict(planet);
            default:
                throw new NotImplementedException("Wait");
        }
    }

    public Building GetBuilding(BuildingType type, Planet_Inhabitable planet)
    {
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                return new ColonizationCenter(planet);
            case BuildingType.AlloySmelter:
                return new AlloySmelter(planet);
            case BuildingType.HouseOfWisdom:
                return new HouseOfWisdom(planet);
            case BuildingType.PlanetaryMarket:
                return new PlanetaryMarket(planet);
            case BuildingType.GovernmentHall:
                return new GovernmentHall(planet);
            case BuildingType.PoliceStation:
                return new PoliceStation(planet);
            case BuildingType.ControlTower:
                return new ControlTower(planet);
            case BuildingType.FleetTrainingCenter:
                return new FleetTrainingCenter(planet);
            case BuildingType.CommandCenter:
                return new CommandCenter(planet);
            case BuildingType.MineralExtractor:
                return new MineralExtractor(planet);
            case BuildingType.BioPlant:
                return new BioPlant(planet);
            default:
                throw new NotImplementedException("Wait");
        }
    }

    public int GetConstructionTime(BuildingType type)
    {
        int result;
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                result = 0;
                break;
            case BuildingType.PlanetaryCapital:
                result = 480;
                break;
            case BuildingType.AlloySmelter:
            case BuildingType.HouseOfWisdom:
            case BuildingType.GovernmentHall:
                result = 240;
                break;
            case BuildingType.AlloyFoundry:
            case BuildingType.ResearchLab:
            case BuildingType.PlanetaryMarket:
            case BuildingType.PlanetaryCommHub:
            case BuildingType.FleetTrainingCenter:
            case BuildingType.CommandCenter:
            case BuildingType.BioPlant:
            case BuildingType.MineralExtractor:
                result = 360;
                break;
            case BuildingType.PoliceStation:
            case BuildingType.ControlTower:
                result = 180;
                break;
            default:
                throw new InvalidOperationException("Invalid building type detected.");
        }
        return (int)(result / planet.game.constructionTimeModifier);
    }

    public int GetConstructionTime(DistrictType type)
    {
        int result;
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Electricity:
            case DistrictType.Mineral:
                result = 180;
                break;
            case DistrictType.House:
                result = 240;
                break;
            default:
                throw new InvalidOperationException("Invalid district type detected!");
        }
        return (int)(result / planet.game.constructionTimeModifier);
    }

    public int GetConstructionCost(BuildingType type)
    {
        int result;
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                result = 0;
                break;
            case BuildingType.PlanetaryCapital:
                result = 1000;
                break;
            case BuildingType.AlloySmelter:
            case BuildingType.HouseOfWisdom:
            case BuildingType.GovernmentHall:
            case BuildingType.PoliceStation:
            case BuildingType.ControlTower:
                result = 200;
                break;
            case BuildingType.AlloyFoundry:
            case BuildingType.ResearchLab:
            case BuildingType.PlanetaryCommHub:
                result = 500;
                break;
            case BuildingType.PlanetaryMarket:
            case BuildingType.CommandCenter:
            case BuildingType.FleetTrainingCenter:
            case BuildingType.BioPlant:
            case BuildingType.MineralExtractor:
                result = 300;
                break;
            default:
                throw new InvalidOperationException("Invalid BuildingType detected!");
        }
        return (int)(result / planet.game.constructionCostModifier);
    }

    public int GetConstructionCost(DistrictType type)
    {
        int result;
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Electricity:
            case DistrictType.Mineral:
                result = 150;
                break;
            case DistrictType.House:
                result = 200;
                break;
            default:
                throw new InvalidOperationException("Invalid DistrictType detected!");
        }
        return (int)(result / planet.game.constructionCostModifier);
    }

    public bool IsMineralEnough(BuildingType type)
    {
        return planet.planetaryResources.mineral > GetConstructionCost(type);
    }

    public bool IsMineralEnough(DistrictType type)
    {
        return planet.planetaryResources.mineral > GetConstructionCost(type);
    }
}
