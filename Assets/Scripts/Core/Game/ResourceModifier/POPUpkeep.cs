public class POPUpkeep : GlobalResourceModifiers
{

    public POP pop;

    public POPUpkeep(POP pop) : base((GlobalResourceType.Food, 1), ModifierType.Upkeep)
    {
        this.pop = pop;
    }
}
