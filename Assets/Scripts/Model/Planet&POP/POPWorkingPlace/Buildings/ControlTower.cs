using System;

public class ControlTower : Building, IUpgradable
{
    public ControlTower(Planet_Inhabitable planet) : base(planet, 0.5f)
    {
        name = "Control Tower";
        buildingType = BuildingType.ControlTower;
        InitiallizePOPWorkingList(1);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 2), null));
        workingPOPSlotList[0].job = Job.Admiral;       
    }

    public bool IsUpgradable()
    {
        return planet.workingPlaceFactory.IsMineralEnough(BuildingType.PlanetaryCommHub);
    }

    public void Upgrade()
    {
        if (!IsUpgradable()) throw new InvalidOperationException("The upgrade condition is not met!");

        POPWorkingSlot[] newArray = new POPWorkingSlot[2];
        for (int i = 0; i < newArray.Length; i++)
            newArray[i] = new POPWorkingSlot();

        for (int i = 0; i < workingPOPSlotList.Length; i++)
            newArray[i] = workingPOPSlotList[i];

        workingPOPSlotList = newArray;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 2), null));
        workingPOPSlotList[1].job = Job.Admiral;

        name = "Planetary Comm Hub";
        buildingType = BuildingType.PlanetaryCommHub;
        baseUpkeep.amount = 1;
    }

}