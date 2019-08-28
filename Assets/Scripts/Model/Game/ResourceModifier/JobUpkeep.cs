public class JobUpkeep : GlobalResourceModifiers
{

    public POP pop;
    private float _baseJobModifier => 1;

    public JobUpkeep((GlobalResourceType, float) v, POP pop) : base(v, ModifierType.Upkeep)
    {
        this.pop = pop;
    }

    public override float GetModifier()
    {
        return _baseJobModifier;
    }
}
