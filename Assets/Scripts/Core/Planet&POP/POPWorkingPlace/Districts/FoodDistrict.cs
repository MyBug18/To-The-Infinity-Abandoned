public class FoodDistrict : District
{

    public FoodDistrict(Planet_Inhabitable planet) : base(planet)
    {
        name = "Food District";

        districtType = DistrictType.Food;
        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Food, 6f), null));
        workingPOPSlotList[0].job = Job.Farmer;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Food, 6f), null));
        workingPOPSlotList[1].job = Job.Farmer;
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentFoodDistrictNum++;
        planet.housing += 3;
    }

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentFoodDistrictNum--;
        planet.housing -= 3;
    }
}
