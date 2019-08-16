using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class WorkingPlaceFactory
{
    public static District BuildDistrict(DistrictType type, Planet_Inhabitable planet)
    {
        switch(type)
        {
            case DistrictType.Fuel:
                return new FuelDistrict(planet);
            case DistrictType.Mineral:
                return new MineralDistrict(planet);
            default:
                throw new NotImplementedException("Wait");
        }
    }
}
