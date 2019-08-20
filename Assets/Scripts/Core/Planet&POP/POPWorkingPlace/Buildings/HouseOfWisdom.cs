using System;

public class HouseOfWisdom : Building, IUpgradable
{
    public HouseOfWisdom(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "House of Wisdom";
        buildingType = BuildingType.HouseOfWisdom;
        buildingCost = 200;

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Physics, 4f), null));
        workingPOPSlotList[0].job = Job.Physicist;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Sociology, 4f), null));
        workingPOPSlotList[1].job = Job.Sociologist;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[2].yields.Add(new JobYield((GlobalResourceType.Engineering, 4f), null));
        workingPOPSlotList[2].job = Job.Engineer;
    }

    public bool IsUpgradable()
    {
        return true;
    }

    public void Upgrade()
    {
        if (!IsUpgradable()) throw new InvalidOperationException("The upgrade condition is not met!");

        POPWorkingSlot[] newArray = new POPWorkingSlot[6];
        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = new POPWorkingSlot();

        for (int i = 0; i < workingPOPSlotList.Length; i++)
            newArray[i] = workingPOPSlotList[i];

        workingPOPSlotList = newArray; // Resizes workingPOPSlotList.

        workingPOPSlotList[3].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[3].yields.Add(new JobYield((GlobalResourceType.Physics, 4f), null));
        workingPOPSlotList[3].job = Job.Physicist;

        workingPOPSlotList[4].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[4].yields.Add(new JobYield((GlobalResourceType.Sociology, 4f), null));
        workingPOPSlotList[4].job = Job.Sociologist;

        workingPOPSlotList[5].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null)); // Initiallize Technician job slot.
        workingPOPSlotList[5].yields.Add(new JobYield((GlobalResourceType.Engineering, 4f), null));
        workingPOPSlotList[5].job = Job.Engineer;

        name = "Research Lab";
        buildingType = BuildingType.ResearchLab;
        baseUpkeep.amount = 2;
    }

    public int GetUpgradeCost()
    {
        return 500;
    }
}