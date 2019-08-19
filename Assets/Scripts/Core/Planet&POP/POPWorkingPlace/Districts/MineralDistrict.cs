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

        workingPOPList[0].job = Job.Miner;
        workingPOPList[1].job = Job.Miner;

        List<JobUpkeep> upkeeps0 = new List<JobUpkeep>();
        JobUpkeep _minerUpkeep0 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps0.Add(_minerUpkeep0);
        workingPOPList[0].upkeeps = upkeeps0;

        List<JobUpkeep> upkeeps1 = new List<JobUpkeep>();
        JobUpkeep _minerUpkeep1 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps1.Add(_minerUpkeep1);
        workingPOPList[1].upkeeps = upkeeps1;

        List<JobYield> yield0 = new List<JobYield>();
        JobYield _minerYield0 = new JobYield((GlobalResourceType.Mineral, 6f), null);
        yield0.Add(_minerYield0);
        workingPOPList[0].yields = yield0;

        List<JobYield> yield1 = new List<JobYield>();
        JobYield _minerYield1 = new JobYield((GlobalResourceType.Mineral, 6f), null);
        yield1.Add(_minerYield1);
        workingPOPList[1].yields = yield1;
    }

    public override void OnConstructing()
    {
        base.OnConstructing();
        planet.currentMineralDistrictNum++;
        planet.housing += 3;
    }

    public override void OnDemolishing()
    {
        base.OnDemolishing();
        planet.currentMineralDistrictNum--;
        planet.housing -= 3;
    }
}
