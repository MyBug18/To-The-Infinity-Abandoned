using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

public abstract class POPWorkingPlace
{
    public POPWorkingPlace(WorkingPlaceType type, Planet_Inhabitable planet)
    {
        this.planet = planet;

        this.type = type;
        OnConstructing();       
    }

    public string name;

    public Planet_Inhabitable planet { get; private set; } // The planet where this building is.
    
    public int workingPOPSlotNumber { get; protected set; }

    public POPWorkingSlot[] workingPOPList { get; protected set; }

    public WorkingPlaceType type { get; private set; }

    public WorkingPlaceBaseUpkeep baseUpkeep { get; protected set; }

    public virtual void OnConstructing()
    {
        baseUpkeep = new WorkingPlaceBaseUpkeep(this);
        planet.planetBaseUpkeeps.Add(baseUpkeep); // Add Upkeep of this building.
    }

    public virtual bool IsDemolishable() // Must be checked before demolishing this.
    {
        bool result = true;
        for (int i = 0; i < workingPOPSlotNumber; i++)
        {
            if (workingPOPList[i].pop != null)
                result = false;
        }
        return result;
    }

    public virtual void OnDemolishing() // Must be called before demolishing this.
    {
        planet.planetBaseUpkeeps.Remove(baseUpkeep); // Remove Upkeep of this building.
    }

    public virtual void AllocatePOP(POP pop, int slotNum) // Allocates POP with slot number, and Add Upkeeps to Global Modifier.
    {
        workingPOPList[slotNum].isPOPTrainingForHere = false;
        Debug.Log("Allocating " + pop.name + " to " + slotNum + "th slot of " + name + " as " + workingPOPList[slotNum].job);
        workingPOPList[slotNum].pop = pop;

        foreach (var upkeep in workingPOPList[slotNum].upkeeps)
        {
            upkeep.pop = pop;
            planet.planetJobUpkeeps.Add(upkeep);
        }

        foreach (var yield in workingPOPList[slotNum].yields)
        {
            yield.pop = pop;
            planet.planetJobYields.Add(yield);
        }

        Debug.Log(this);
    }

    public virtual void MovePOPJob(int fromSlotNum, POPWorkingPlace workingPlace, int toSlotNum) // Removes POPs from the slot, puts it in the training list, and remove Upkeeps.
    {
        if (workingPlace.workingPOPList[toSlotNum].pop != null)
            throw new InvalidOperationException("Trying to move to already occupied slot!");

        if (workingPlace.workingPOPList[toSlotNum].isPOPTrainingForHere)
            throw new InvalidOperationException("Someone is already training for there!");

        POPWorkingSlot slot = workingPOPList[fromSlotNum];

        if (slot.pop == null) throw new InvalidOperationException("Trying to move pop which doesn't exist! (On " + planet.name + ", " + fromSlotNum + "th slot of " + name + ")");


        POP pop = slot.pop;
        foreach (var upkeep in slot.upkeeps)
        {
            upkeep.pop = null;
            planet.planetJobUpkeeps.Remove(upkeep);
        }

        foreach (var yield in workingPOPList[fromSlotNum].yields)
        {
            yield.pop = null;
            planet.planetJobYields.Remove(yield);
        }

        workingPOPList[fromSlotNum].pop = null;
        
        pop.StartTraining(workingPlace, toSlotNum);
    }

    public Job GetJobOfWorkingSlot(int slotNum) // Gets a Job with slot number.
    {
        return workingPOPList[slotNum].job;
    }

    public override string ToString()
    {
        string result = "";
        string popStatus = "";

        for(int i = 0; i < workingPOPSlotNumber; i++)
        {
            var slot = workingPOPList[i];
            if (slot.pop != null)
            {
                string upkeepStatus = " Upkeeps: ";
                foreach (var u in slot.upkeeps)
                    upkeepStatus += u + " ";


                string p = i + "th slot: " + slot.pop.name + ": " + slot.job + upkeepStatus + "\n";
                popStatus += p;
            }
        }

        result = name + " on " + planet.name + "\n" + popStatus;

        return result;
    }

    protected void InitiallizePOPWorkingList(int n)
    {
        workingPOPSlotNumber = n;
        workingPOPList = new POPWorkingSlot[workingPOPSlotNumber];
        for (int i = 0; i < workingPOPSlotNumber; i++)
            workingPOPList[i] = new POPWorkingSlot();
    }
}

public enum WorkingPlaceType
{
    Building, District
}
