public abstract class GlobalResourceChanges
{
    public ChangeType changeType { get; private set; }
    public GlobalResourceType resourceType;
    public float amount; // must be positive.
    public abstract float GetModifier();

    public (GlobalResourceType type, float amount) value => (resourceType, amount * GetModifier());

    public GlobalResourceChanges(ChangeType type)
    {
        changeType = type;
    }

    public GlobalResourceChanges((GlobalResourceType, float) v, ChangeType type)
    {
        changeType = type;
        resourceType = v.Item1;
        amount = v.Item2;
    }

    public override string ToString()
    {
        string result;
        result = changeType + ": (" + resourceType + ", " + amount + ")"; 
        return result;
    }
}

public enum ChangeType
{
    Yield,
    Upkeep
}
