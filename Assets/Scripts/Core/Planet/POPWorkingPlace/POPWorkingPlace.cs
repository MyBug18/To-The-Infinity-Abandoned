using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

public abstract class POPWorkingPlace
{
    public string name;

    public Planet_Inhabitable planet { get; private set; } // The planet where this building is.

    //public POP[] workingPOPList { get; protected set; }
    public int workingPOPSlotNumber { get; protected set; }
    //public List<JobUpkeep> jobUpkeeps { get; protected set; }

    public (POP pop, Job job, List<JobUpkeep> upkeeps)[] workingPOPList { get; protected set; }

    public WorkingPlaceType type { get; private set; }

    public WorkingPlaceBaseUpkeep baseUpkeep { get; protected set; }

    public bool isDemolishable() // Must be checked before demolishing this.
    {
        bool result = true;
        for (int i = 0; i < workingPOPSlotNumber; i++)
        {
            if (workingPOPList[i].pop != null)
                result = false;
        }
        return result;
    }

    public POPWorkingPlace(WorkingPlaceType type, Planet_Inhabitable planet)
    {
        this.planet = planet;

        this.type = type;

        baseUpkeep = new WorkingPlaceBaseUpkeep(this);
        planet.planetBaseUpkeeps.Add(baseUpkeep); // Add Upkeep of this building.
    }

    public virtual void AllocatePOP(POP pop, int slotNum) // Allocates POP with slot number, and Add Upkeeps to Global Modifier.
    {
        //(POP, Job, List<JobUpkeep>) slot = workingPOPList[slotNum];

        Debug.Log("Allocating " + pop.name + " to " + slotNum + "th slot of " + name + " as " + workingPOPList[slotNum].Item2);
        workingPOPList[slotNum].Item1 = pop;
        foreach (var upkeep in workingPOPList[slotNum].Item3)
        {
            upkeep.pop = pop;
            planet.planetJobUpkeeps.Add(upkeep);
        }

        Debug.Log(this);
    }

    public virtual void MovePOPJob(int slotNum, (POPWorkingPlace, int) futureWorkingPlace) // Removes POPs from the slot, puts it in the training list, and remove Upkeeps.
    {
        if (futureWorkingPlace.Item1.workingPOPList[futureWorkingPlace.Item2].pop != null)
            throw new InvalidOperationException("Trying to move to already occupied slot!");

        (POP pop, Job job, List<JobUpkeep> upkeeps) slot = workingPOPList[slotNum];

        if (slot.pop == null) throw new InvalidOperationException("Trying to move pop which doesn't exist! (On " + planet.name + ", " + slotNum + "th slot of " + name + ")");


        POP pop = slot.pop;
        foreach (var upkeep in slot.upkeeps)
        {
            upkeep.pop = null;
            planet.planetJobUpkeeps.Remove(upkeep);
        }
        workingPOPList[slotNum].pop = null;
        
        pop.StartTraining(futureWorkingPlace);
    }

    public Job GetJobOfWorkingSlot(int slotNum) // Gets a Job with slot number.
    {
        return workingPOPList[slotNum].job;
    }

    public virtual void BeforeDemolish() // Must be called before demolishing this.
    {
        planet.planetBaseUpkeeps.Remove(baseUpkeep); // Remove Upkeep of this building.
    }

    public (POP pop, Job job, List<JobUpkeep> upkeeps) GetNthSlot(int slotNum)
    {
        return workingPOPList[slotNum];
    }

    public override string ToString()
    {
        string result = "";
        string popStatus = "";

        for(int i = 0; i < workingPOPSlotNumber; i++)
        {
            var (pop, job, upkeeps) = workingPOPList[i];
            if (pop != null)
            {
                string upkeepStatus = " Upkeeps: ";
                foreach (var u in upkeeps)
                    upkeepStatus += u + " ";


                string p = i + "th slot: " + pop.name + ": " + job + upkeepStatus + "\n";
                popStatus += p;
            }
        }

        result = name + " on " + planet.name + "\n" + popStatus;

        return result;
    }
}

public enum WorkingPlaceType
{
    Building, District
}
