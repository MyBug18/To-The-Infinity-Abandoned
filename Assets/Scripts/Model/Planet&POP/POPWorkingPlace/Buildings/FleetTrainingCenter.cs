public class FleetTrainingCenter : Building
{
    public static BuildingType type => BuildingType.FleetTrainingCenter;
    public FleetTrainingCenter(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Fleet Training Center";
        buildingType = BuildingType.FleetTrainingCenter;
        InitiallizePOPWorkingList(3);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[0].job = Job.Staff;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[1].job = Job.Staff;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[2].job = Job.Staff;
    }
}