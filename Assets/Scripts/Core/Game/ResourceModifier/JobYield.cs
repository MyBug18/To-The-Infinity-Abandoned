using System.Collections;
using System.Collections.Generic;

public class JobYield : GlobalResourceModifiers
{

    public POP pop;

    public JobYield((GlobalResourceType, float) v, POP pop) : base(v, ModifierType.Yield)
    {
        this.pop = pop;
    }
}
