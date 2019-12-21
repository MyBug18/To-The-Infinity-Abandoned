public class JobUpkeep : GlobalResourceChanges
{

    public POP pop;
    private float _baseJobModifier => 1;

    public JobUpkeep((GlobalResourceType, float) v, POP pop) : base(v, ChangeType.Upkeep)
    {
        this.pop = pop;
    }

    public override float GetModifier()
    {
        return _baseJobModifier;
    }
}
