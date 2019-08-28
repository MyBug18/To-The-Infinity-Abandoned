public abstract class District : POPWorkingPlace
{
    public DistrictType districtType { get; protected set; }
    
    public District(Planet_Inhabitable planet) : base(WorkingPlaceType.District, planet) // Every district's upkeep is 0.5 fuel.
    {
        baseUpkeep.resourceType = GlobalResourceType.Electricity;
        baseUpkeep.amount = 0.5f;
    }
}

public enum DistrictType
{
    Fuel, Mineral, Food, House
}
