using System;

public class ColonizationCenter : Building, IUpgradable
{
    public ColonizationCenter(Planet_Inhabitable planet) : base(planet, 1f)
    {
        name = "Colonization Center";

        buildingType = BuildingType.ColonizationCenter;
        InitiallizePOPWorkingList(6);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Fuel, 1.5f), null));
        workingPOPSlotList[0].job = Job.Technician;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Mineral, 6f), null));
        workingPOPSlotList[1].job = Job.Miner;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[2].yields.Add(new JobYield((GlobalResourceType.Food, 6f), null));
        workingPOPSlotList[2].job = Job.Farmer;

        workingPOPSlotList[3].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[3].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[3].job = Job.Clerk;

        workingPOPSlotList[4].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[4].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[4].job = Job.Clerk;

        workingPOPSlotList[5].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[5].job = Job.Enforcer;

    }

    public bool IsUpgradable()
    {
        return planet.pops.Count >= 50;
    }

    public void Upgrade()
    {
        if (!IsUpgradable()) throw new InvalidOperationException("The upgrade condition is not met!");

        POPWorkingSlot[] newArray = new POPWorkingSlot[12];
        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = new POPWorkingSlot();

        for (int i = 0; i < workingPOPSlotList.Length; i++)
            newArray[i] = workingPOPSlotList[i];

        workingPOPSlotList = newArray; // Resizes workingPOPSlotList.

        workingPOPSlotList[6].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[6].yields.Add(new JobYield((GlobalResourceType.Fuel, 1.5f), null));
        workingPOPSlotList[6].job = Job.Technician;

        workingPOPSlotList[7].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[7].yields.Add(new JobYield((GlobalResourceType.Mineral, 6f), null));
        workingPOPSlotList[7].job = Job.Miner;

        workingPOPSlotList[8].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[8].yields.Add(new JobYield((GlobalResourceType.Food, 6f), null));
        workingPOPSlotList[8].job = Job.Farmer;

        workingPOPSlotList[9].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[9].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[9].job = Job.Clerk;

        workingPOPSlotList[10].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[10].job = Job.Enforcer;

        workingPOPSlotList[11].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[11].job = Job.Administrator;

        Array.Sort(workingPOPSlotList, (POPWorkingSlot a, POPWorkingSlot b) => a.job.CompareTo(b.job));

        name = "Planetary Capital";
        buildingType = BuildingType.PlanetaryCapital;
        baseUpkeep.amount = 3;
    }

    public int GetUpgradeCost()
    {
        return 1000;
    }
}