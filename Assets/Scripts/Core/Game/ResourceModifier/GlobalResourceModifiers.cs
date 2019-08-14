public abstract class GlobalResourceModifiers
{

    public ModifierType modifierType;
    public GlobalResourceType resourceType;
    public float amount; // must be positive.

    public GlobalResourceModifiers(ModifierType type)
    {
        modifierType = type;
    }

    public GlobalResourceModifiers((GlobalResourceType, float) v, ModifierType type)
    {
        modifierType = type;
        resourceType = v.Item1;
        amount = v.Item2;
    }
}

public enum ModifierType
{
    Yield,
    Upkeep
}
