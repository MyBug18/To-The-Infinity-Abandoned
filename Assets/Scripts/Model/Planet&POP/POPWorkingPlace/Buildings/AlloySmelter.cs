using System;

public class AlloySmelter : Building, IUpgradable
{
    public AlloySmelter(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Alloy Smelter";
        buildingType = BuildingType.AlloySmelter;
        InitiallizePOPWorkingList(3);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 4f), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Alloy, 2f), null));
        workingPOPSlotList[0].job = Job.Technician;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 4f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Alloy, 2f), null));
        workingPOPSlotList[1].job = Job.Technician;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 4f), null));
        workingPOPSlotList[2].yields.Add(new JobYield((GlobalResourceType.Alloy, 2f), null));
        workingPOPSlotList[2].job = Job.Technician;
    }

    public bool IsUpgradable()
    {
        return true;
    }

    public void Upgrade()
    {
        if (!IsUpgradable()) throw new InvalidOperationException("The upgrade condition is not met!");

        POPWorkingSlot[] newArray = new POPWorkingSlot[5];
        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = new POPWorkingSlot();

        for (int i = 0; i < workingPOPSlotList.Length; i++)
            newArray[i] = workingPOPSlotList[i];

        workingPOPSlotList = newArray; // Resizes workingPOPSlotList.

        workingPOPSlotList[3].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[3].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 4f), null));
        workingPOPSlotList[3].yields.Add(new JobYield((GlobalResourceType.Alloy, 2f), null));
        workingPOPSlotList[3].job = Job.Technician;

        workingPOPSlotList[4].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[4].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 4f), null));
        workingPOPSlotList[4].yields.Add(new JobYield((GlobalResourceType.Alloy, 2f), null));
        workingPOPSlotList[4].job = Job.Technician;

        name = "Alloy Foundry";
        buildingType = BuildingType.AlloyFoundry;
        baseUpkeep.amount = 2;
    }
    
}