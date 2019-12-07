using System.Collections.Generic;
using System;

using UnityEngine;

public abstract class POPWorkingPlace
{
    protected POPWorkingPlace(WorkingPlaceType type, Planet_Inhabitable planet)
    {
        this.planet = planet;
        this.type = type;
        OnConstructing();       
    }

    public string name;

    public Planet_Inhabitable planet { get; private set; } // The planet where this building is.
    
    public int workingPOPSlotNumber { get; protected set; }

    public POPWorkingSlot[] workingPOPSlotList { get; protected set; }

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
            if (workingPOPSlotList[i].pop != null)
                result = false;
        }
        return result;
    }

    public virtual void OnDemolishing() // Must be called before demolishing this.
    {
        planet.planetBaseUpkeeps.Remove(baseUpkeep); // Remove Upkeep of this building.
    }

    public Job GetJobOfWorkingSlot(int slotNum) // Gets a Job with slot number.
    {
        return workingPOPSlotList[slotNum].job;
    }

    public override string ToString()
    {
        string result = "";
        string popStatus = "";

        for(int i = 0; i < workingPOPSlotNumber; i++)
        {
            var slot = workingPOPSlotList[i];
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
        workingPOPSlotList = new POPWorkingSlot[workingPOPSlotNumber];
        for (int i = 0; i < workingPOPSlotNumber; i++)
            workingPOPSlotList[i] = new POPWorkingSlot();
    }
}

public enum WorkingPlaceType
{
    Building, District
}
public class POPWorkingSlot
{
    public POP pop;
    public Job job;
    public List<JobUpkeep> upkeeps = new List<JobUpkeep>();
    public List<JobYield> yields = new List<JobYield>();
    public bool isPOPTrainingForHere = false;
}