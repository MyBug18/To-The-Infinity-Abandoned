using System;
using System.Collections;
using System.Collections.Generic;

public class FuelDistrict : District
{

    public FuelDistrict() : base()
    {
        districtType = DistrictType.Fuel;
        workingPOPSlotNumber = 2;
        workingPOPList = new (POP, Job, List<JobUpkeep>)[workingPOPSlotNumber];

        workingPOPList[0].job = Job.Technician;
        workingPOPList[1].job = Job.Technician;

        List<JobUpkeep> upkeeps0 = new List<JobUpkeep>();
        JobUpkeep _fuelTechUpkeep0 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps0.Add(_fuelTechUpkeep0);
        workingPOPList[1].upkeeps = upkeeps0;

        List<JobUpkeep> upkeeps1 = new List<JobUpkeep>();
        JobUpkeep _fuelTechUpkeep1 = new JobUpkeep((GlobalResourceType.Money, 0.5f), null);
        upkeeps1.Add(_fuelTechUpkeep1);
        workingPOPList[1].upkeeps = upkeeps1;
    }
    


}
