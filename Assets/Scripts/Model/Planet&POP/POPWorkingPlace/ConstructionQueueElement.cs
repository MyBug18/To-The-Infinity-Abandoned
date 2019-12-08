using System;

public class ConstructionQueueElement
{
    public bool isBuilding; // true if building; false if district
    public int type; // enum type. casted depending on isbuilding.
    public int remainTime;
    public Building fromUpgrade;

    public Action OnTimerEnded;

    public ConstructionQueueElement(bool isBuilding, int type, int remainTime, Building fromUpgrade, Action OnTimerEnded)
    {
        this.isBuilding = isBuilding;
        this.type = type;
        this.remainTime = remainTime;
        this.fromUpgrade = fromUpgrade;
        this.OnTimerEnded = OnTimerEnded;
    }
}