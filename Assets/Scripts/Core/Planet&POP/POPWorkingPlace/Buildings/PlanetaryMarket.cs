using System;

public class PlanetaryMarket : Building
{
    public PlanetaryMarket(Planet_Inhabitable planet) : base(planet, 1.5f)
    {
        name = "Planetary Market";
        buildingType = BuildingType.PlanetaryMarket;

        InitiallizePOPWorkingList(5);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[0].job = Job.Clerk;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[1].job = Job.Clerk;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[2].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[2].job = Job.Clerk;

        workingPOPSlotList[3].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[3].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[3].job = Job.Clerk;

        workingPOPSlotList[4].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[4].yields.Add(new JobYield((GlobalResourceType.Money, 3f), null));
        workingPOPSlotList[4].job = Job.Clerk;
    }
}