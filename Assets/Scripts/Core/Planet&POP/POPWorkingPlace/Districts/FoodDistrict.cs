using System;
using System.Collections;
using System.Collections.Generic;

public class FoodDistrict : District
{

    public FoodDistrict(Planet_Inhabitable planet) : base(planet)
    {
        name = "Food District";

        districtType = DistrictType.Food;
        InitiallizePOPWorkingList(2);

        workingPOPList[0].job = Job.Farmer;
        workingPOPList[1].job = Job.Farmer;

        List<JobUpkeep> upkeeps0 = new List<JobUpkeep>();
        JobUpkeep _farmerUpkeep0 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps0.Add(_farmerUpkeep0);
        workingPOPList[0].upkeeps = upkeeps0;

        List<JobUpkeep> upkeeps1 = new List<JobUpkeep>();
        JobUpkeep _farmerUpkeep1 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps1.Add(_farmerUpkeep1);
        workingPOPList[1].upkeeps = upkeeps1;

        List<JobYield> yield0 = new List<JobYield>();
        JobYield _farmerYield0 = new JobYield((GlobalResourceType.Food, 6f), null);
        yield0.Add(_farmerYield0);
        workingPOPList[0].yields = yield0;

        List<JobYield> yield1 = new List<JobYield>();
        JobYield _farmerYield1 = new JobYield((GlobalResourceType.Food, 6f), null);
        yield1.Add(_farmerYield1);
        workingPOPList[1].yields = yield1;
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
