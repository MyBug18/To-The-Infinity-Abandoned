using System.Collections;
using System.Collections.Generic;

public class JobUpkeep : GlobalResourceModifiers
{

    public POP pop;

    public JobUpkeep((GlobalResourceType, float) v, POP pop) : base(v, ModifierType.Upkeep)
    {
        this.pop = pop;
    }
}
