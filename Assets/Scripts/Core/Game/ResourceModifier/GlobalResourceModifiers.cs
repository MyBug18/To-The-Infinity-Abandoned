using System;

public abstract class GlobalResourceModifiers
{

    public ModifierType modifierType { get; private set; }
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

    public override string ToString()
    {
        string result;
        result = modifierType + ": (" + resourceType + ", " + amount + ")"; 
        return result;
    }
}

public enum ModifierType
{
    Yield,
    Upkeep
}
