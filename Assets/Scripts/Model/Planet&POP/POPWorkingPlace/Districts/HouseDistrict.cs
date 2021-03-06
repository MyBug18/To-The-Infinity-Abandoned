﻿public class HouseDistrict : District
{
    public HouseDistrict(Planet_Inhabitable planet) : base(planet)
    {
        districtType = DistrictType.House;
        InitiallizePOPWorkingList(0);
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentHouseDistrictNum++;
        planet.plannedHouseDistrictNum--;
        planet.providedHousing += 8;
    }

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentHouseDistrictNum--;
        planet.providedHousing -= 8;
    }
}