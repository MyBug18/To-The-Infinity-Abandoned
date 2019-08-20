public class ConstructionQueueElement
{
    public bool isBuilding;
    public int type;
    public int remainTime;
    public Building fromUpgrade;

    public ConstructionQueueElement(bool isBuilding, int type, int remainTime, Building fromUpgrade)
    {
        this.isBuilding = isBuilding;
        this.type = type;
        this.remainTime = remainTime;
        this.fromUpgrade = fromUpgrade;
    }
}