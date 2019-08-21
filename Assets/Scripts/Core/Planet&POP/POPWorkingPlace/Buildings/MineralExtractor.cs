public class MineralExtractor : Building
{
    public MineralExtractor(Planet_Inhabitable planet) : base(planet, 1)
    {
        name = "Mineral Extractor";
        buildingType = BuildingType.MineralExtractor;

        InitiallizePOPWorkingList(2);

        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[0].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 3), null));
        workingPOPSlotList[0].yields.Add(new JobYield((GlobalResourceType.Fuel, 1), null));
        workingPOPSlotList[0].job = Job.Miner;

        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Money, 0.5f), null));
        workingPOPSlotList[1].upkeeps.Add(new JobUpkeep((GlobalResourceType.Mineral, 3), null));
        workingPOPSlotList[1].yields.Add(new JobYield((GlobalResourceType.Fuel, 1), null));
        workingPOPSlotList[1].job = Job.Miner;
    }
}