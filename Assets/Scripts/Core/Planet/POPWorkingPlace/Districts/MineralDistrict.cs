using System;
using System.Collections;
using System.Collections.Generic;

public class MineralDistrict : District
{

    public MineralDistrict() : base()
    {
        districtType = DistrictType.Mineral;
        workingPOPSlotNumber = 2;
        workingPOPList = new (POP, Job, List<JobUpkeep>)[workingPOPSlotNumber];

        workingPOPList[0].job = Job.Miner;
        workingPOPList[1].job = Job.Miner;

        List<JobUpkeep> upkeeps0 = new List<JobUpkeep>();
        JobUpkeep _minerUpkeep0 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps0.Add(_minerUpkeep0);
        workingPOPList[1].upkeeps = upkeeps0;

        List<JobUpkeep> upkeeps1 = new List<JobUpkeep>();
        JobUpkeep _minerUpkeep1 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps1.Add(_minerUpkeep1);
        workingPOPList[1].upkeeps = upkeeps1;
    }



}
