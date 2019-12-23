public class BioPlant : Building
{
    public static BuildingType type => BuildingType.BioPlant;
    public BioPlant(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Mineral Extractor";
        buildingType = BuildingType.BioPlant;

        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Food, 5), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Electricity, 1), null));
        workingPOPSlotList[0].job = Job.Farmer;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Food, 5), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Electricity, 1), null));
        workingPOPSlotList[1].job = Job.Farmer;
    }
}