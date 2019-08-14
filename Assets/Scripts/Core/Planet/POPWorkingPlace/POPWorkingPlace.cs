using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class POPWorkingPlace
{
    public Planet planet { get; private set; } // The planet where this building is.

    //public POP[] workingPOPList { get; protected set; }
    public int workingPOPSlotNumber { get; protected set; }
    //public List<JobUpkeep> jobUpkeeps { get; protected set; }

    public (POP pop, Job job, List<JobUpkeep> upkeeps)[] workingPOPList { get; protected set; }

    public WorkingPlaceType type { get; private set; }

    public WorkingPlaceBaseUpkeep baseUpkeep { get; protected set; }

    public bool isAbleToBeDemolished()
    {
        bool result = true;
        for (int i = 0; i < workingPOPSlotNumber; i++)
        {
            if (workingPOPList[i].pop != null)
                result = false;
        }
        return result;
    }

    public POPWorkingPlace(WorkingPlaceType type)
    {
        this.type = type;

        baseUpkeep = new WorkingPlaceBaseUpkeep(this);


        Game.everyResourceModifiers.Add(baseUpkeep); // Add Upkeep of this building.
    }


    public virtual void AllocatePOP(POP pop, int slotNum) // Allocates POP with slot number, and Add Upkeeps to Global Modifier.
    {
        (POP, Job, List<JobUpkeep>) slot = workingPOPList[slotNum];
        slot.Item1 = pop;
        foreach (var upkeep in slot.Item3)
        {
            upkeep.pop = pop;
            Game.everyResourceModifiers.Add(upkeep);
        }
    }

    public virtual void MovePOPJob(int slotNum, (POPWorkingPlace, int) futureWorkingPlace) // Removes POPs from the slot, puts it in the training list, and remove Upkeeps from Global Modifier.
    {
        (POP pop, Job job, List<JobUpkeep> upkeeps) slot = workingPOPList[slotNum];
        POP pop = slot.pop;
        foreach (var upkeep in slot.upkeeps)
        {
            upkeep.pop = null;
            Game.everyResourceModifiers.Remove(upkeep);
        }
        slot.pop = null;

        pop.futureWorkingPlace = futureWorkingPlace;
        pop.StartTraining();
    }


    public Job GetJobOfWorkingSlot(int slotNum) // Gets a Job with slot number.
    {
        return workingPOPList[slotNum].job;
    }

    public virtual void BeforeDemolish() // Must be called before demolishing this.
    {
        Game.everyResourceModifiers.Remove(baseUpkeep); // Remove Upkeep of this building.
    }
}

public enum WorkingPlaceType
{
    Building, District
}
