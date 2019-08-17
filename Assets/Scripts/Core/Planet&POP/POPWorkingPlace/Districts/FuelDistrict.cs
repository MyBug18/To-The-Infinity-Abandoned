using System;
using System.Collections;
using System.Collections.Generic;

public class FuelDistrict : District
{

    public FuelDistrict(Planet_Inhabitable planet) : base(planet)
    {
        name = "Fuel District";

        districtType = DistrictType.Fuel;
        workingPOPSlotNumber = 2;
        InitiallizePOPWorkingList();

        workingPOPList[0].job = Job.Technician;
        workingPOPList[1].job = Job.Technician;

        List<JobUpkeep> upkeeps0 = new List<JobUpkeep>();
        JobUpkeep _technicianUpkeep0 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps0.Add(_technicianUpkeep0);
        workingPOPList[0].upkeeps = upkeeps0;

        List<JobUpkeep> upkeeps1 = new List<JobUpkeep>();
        JobUpkeep _technicianUpkeep1 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps1.Add(_technicianUpkeep1);
        workingPOPList[1].upkeeps = upkeeps1;


        List<JobYield> yield0 = new List<JobYield>();
        JobYield _technicianYield0 = new JobYield((GlobalResourceType.Fuel, 1.5f), null);
        yield0.Add(_technicianYield0);
        workingPOPList[0].yields = yield0;

        List<JobYield> yield1 = new List<JobYield>();
        JobYield _technicianYield1 = new JobYield((GlobalResourceType.Fuel, 1.5f), null);
        yield1.Add(_technicianYield1);
        workingPOPList[1].yields = yield1;

    }    

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentFuelDistrictNum--;
        planet.housing -= 3;
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentFuelDistrictNum++;
        planet.housing += 3;
    }
}
