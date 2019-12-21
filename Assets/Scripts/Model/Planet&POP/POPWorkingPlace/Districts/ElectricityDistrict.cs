using System;
using System.Collections;
using System.Collections.Generic;

public class ElectricityDistrict : District
{

    public ElectricityDistrict(Planet_Inhabitable planet) : base(planet)
    {
        name = "Electricity District";

        districtType = DistrictType.Electricity;
        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Electricity, 1.5f), null));
        workingPOPSlotList[0].job = Job.Technician;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Electricity, 1.5f), null));
        workingPOPSlotList[1].job = Job.Technician;

    }    

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentElectricityDistrictNum--;
        planet.providedHousing -= 3;
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentElectricityDistrictNum++;
        planet.plannedElectricityDistrictNum--;
        planet.providedHousing += 3;
    }
}
