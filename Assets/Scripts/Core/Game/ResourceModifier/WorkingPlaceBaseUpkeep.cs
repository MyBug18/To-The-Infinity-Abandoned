﻿using System.Collections;
using System.Collections.Generic;

public class WorkingPlaceBaseUpkeep : GlobalResourceModifiers
{

    public POPWorkingPlace workingPlace;

    public WorkingPlaceBaseUpkeep(POPWorkingPlace workingPlace) : base(ModifierType.Upkeep)
    {
        this.workingPlace = workingPlace;
    }

    public WorkingPlaceBaseUpkeep((GlobalResourceType, float) v, POPWorkingPlace workingPlace) : base(v, ModifierType.Upkeep)
    {
        this.workingPlace = workingPlace;
    }
}