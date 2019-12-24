public class CommandCenter : Building
{
    public CommandCenter(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Fleet Training Center";
        buildingType = BuildingType.FleetTrainingCenter;
        InitiallizePOPWorkingList(3);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1), null));
        workingPOPSlotList[0].job = Job.Soldier;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1), null));
        workingPOPSlotList[1].job = Job.Soldier;

        workingPOPSlotList[2].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1), null));
        workingPOPSlotList[2].job = Job.Soldier;
    }
}