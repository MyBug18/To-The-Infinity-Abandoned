public class JobYield : GlobalResourceChanges
{

    public POP pop;

    public JobYield((GlobalResourceType, float) v, POP pop) : base(v, ChangeType.Yield)
    {
        this.pop = pop;
    }

    public override float GetModifier()
    {
        return 1 + (pop.happiness - 50) / 200;
    }
}
