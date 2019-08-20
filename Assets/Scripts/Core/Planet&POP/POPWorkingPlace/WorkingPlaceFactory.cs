using System;

public static class WorkingPlaceFactory
{
    public static District GetDistrict(DistrictType type, Planet_Inhabitable planet)
    {
        switch(type)
        {
            case DistrictType.Fuel:
                return new FuelDistrict(planet);
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

    public static Building GetBuilding(BuildingType type, Planet_Inhabitable planet)
    {
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                return new ColonizationCenter(planet);
            default:
                throw new NotImplementedException("Wait");
        }
    }

    public static int GetConstructionTime(BuildingType type)
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
        return (int)(result / GameManager.game.constructionTimeModifier);
    }

    public static int GetConstructionTime(DistrictType type)
    {
        int result;
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Fuel:
            case DistrictType.Mineral:
                result = 180;
                break;
            case DistrictType.House:
                result = 240;
                break;
            default:
                throw new InvalidOperationException("Invalid district type detected!");
        }
        return (int)(result / GameManager.game.constructionTimeModifier);
    }

    public static int GetContructionCost(BuildingType type)
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
        return (int)(result / GameManager.game.constructionCostModifier);
    }

    public static int GetConstructionCost(DistrictType type)
    {
        int result;
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Fuel:
            case DistrictType.Mineral:
                result = 150;
                break;
            case DistrictType.House:
                result = 200;
                break;
            default:
                throw new InvalidOperationException("Invalid DistrictType detected!");
        }
        return (int)(result / GameManager.game.constructionCostModifier);
    }
}
