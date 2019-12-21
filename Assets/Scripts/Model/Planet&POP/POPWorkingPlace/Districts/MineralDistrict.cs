using System;
using System.Collections;
using System.Collections.Generic;

public class MineralDistrict : District
{

    public MineralDistrict(Planet_Inhabitable planet) : base(planet)
    {
        name = "Mineral District";

        districtType = DistrictType.Mineral;
        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Mineral, 6f), null));
        workingPOPSlotList[0].job = Job.Miner;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Mineral, 6f), null));
        workingPOPSlotList[1].job = Job.Miner;
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentMineralDistrictNum++;
        planet.plannedMineralDistrictNum--;
        planet.providedHousing += 3;
    }

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentMineralDistrictNum--;
        planet.providedHousing -= 3;
    }
}
