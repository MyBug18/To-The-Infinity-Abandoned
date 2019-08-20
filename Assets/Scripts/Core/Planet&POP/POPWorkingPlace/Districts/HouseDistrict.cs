public class HouseDistrict : District
{
    public HouseDistrict(Planet_Inhabitable planet) : base(planet)
    {
        buildingCost = 200;
        districtType = DistrictType.House;
        InitiallizePOPWorkingList(0);
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentHouseDistrictNum++;
        planet.housing += 8;
    }

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentHouseDistrictNum--;
        planet.housing -= 8;
    }
}