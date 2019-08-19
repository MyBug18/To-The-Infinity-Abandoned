using System.Collections.Generic;


public class POPWorkingSlot
{
    public POP pop;
    public Job job;
    public List<JobUpkeep> upkeeps = new List<JobUpkeep>();
    public List<JobYield> yields = new List<JobYield>();
    public bool isPOPTrainingForHere = false;
    
}
