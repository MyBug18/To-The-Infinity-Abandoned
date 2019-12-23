public class PoliceStation : Building
{
    public static BuildingType type => BuildingType.PoliceStation;
    public PoliceStation(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Police Station";
        buildingType = BuildingType.PoliceStation;

        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[0].job = Job.Enforcer;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 1f), null));
        workingPOPSlotList[1].job = Job.Enforcer;
    }
}