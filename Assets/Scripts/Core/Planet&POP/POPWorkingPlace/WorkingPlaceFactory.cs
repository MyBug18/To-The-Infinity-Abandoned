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
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                return 0;
            case BuildingType.PlanetaryCapital:
                return 480;
            case BuildingType.AlloySmelter:
            case BuildingType.HouseOfWisdom:
            case BuildingType.GovernmentHall:
                return 240;
            case BuildingType.AlloyFoundry:
            case BuildingType.ResearchLab:
            case BuildingType.PlanetaryMarket:
            case BuildingType.PlanetaryCommHub:
            case BuildingType.FleetTrainingCenter:
            case BuildingType.CommandCenter:
            case BuildingType.BioPlant:
            case BuildingType.MineralExtractor:
                return 360;
            case BuildingType.PoliceStation:
            case BuildingType.ControlTower:
                return 180;
            default:
                throw new InvalidOperationException("Invalid building type detected.");
        }
    }

    public static int GetConstructionTime(DistrictType type)
    {
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Fuel:
            case DistrictType.Mineral:
                return 180;
            case DistrictType.House:
                return 240;
            default:
                throw new InvalidOperationException("Invalid district type detected!");
        }
    }

    public static int GetContructionCost(BuildingType type)
    {
        switch(type)
        {
            case BuildingType.ColonizationCenter:
                return 0;
            case BuildingType.PlanetaryCapital:
                return 1000;
            case BuildingType.AlloySmelter:
            case BuildingType.HouseOfWisdom:
            case BuildingType.GovernmentHall:
            case BuildingType.PoliceStation:
            case BuildingType.ControlTower:
                return 200;
            case BuildingType.AlloyFoundry:
            case BuildingType.ResearchLab:
            case BuildingType.PlanetaryCommHub:
                return 500;
            case BuildingType.PlanetaryMarket:
            case BuildingType.CommandCenter:
            case BuildingType.FleetTrainingCenter:
            case BuildingType.BioPlant:
            case BuildingType.MineralExtractor:
                return 300;
            default:
                throw new InvalidOperationException("Invalid BuildingType detected!");
        }
    }

    public static int GetConstructionCost(DistrictType type)
    {
        switch(type)
        {
            case DistrictType.Food:
            case DistrictType.Fuel:
            case DistrictType.Mineral:
                return 150;
            case DistrictType.House:
                return 200;
            default:
                throw new InvalidOperationException("Invalid DistrictType detected!");
        }
    }
}
