public class WorkingPlaceBaseUpkeep : GlobalResourceChanges
{

    public POPWorkingPlace workingPlace;

    public WorkingPlaceBaseUpkeep(POPWorkingPlace workingPlace) : base(ChangeType.Upkeep)
    {
        this.workingPlace = workingPlace;
    }

    public WorkingPlaceBaseUpkeep((GlobalResourceType, float) v, POPWorkingPlace workingPlace) : base(v, ChangeType.Upkeep)
    {
        this.workingPlace = workingPlace;
    }

    public override float GetModifier()
    {
        return 1;
    }
}