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
}
