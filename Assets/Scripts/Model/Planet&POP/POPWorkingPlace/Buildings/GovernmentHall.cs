public class GovernmentHall : Building
{
    public static BuildingType type => BuildingType.GovernmentHall;
    public GovernmentHall(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Government Hall";
        buildingType = BuildingType.GovernmentHall;

        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[0].job = Job.Administrator;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1.5f), null));
        workingPOPSlotList[1].job = Job.Administrator;
    }
}