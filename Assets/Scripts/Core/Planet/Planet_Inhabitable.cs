using System;
using System.Collections;
using System.Collections.Generic;

public class Planet_Inhabitable : Planet
{
    public int maxBuildingSlotNum => 12;

    public int housing { get; private set; }
    public float stability { get; private set; }
    public float crime { get; private set; }
    public int amenity { get; private set; }

    public List<Building> buildings { get; private set; } = new List<Building>();
    public List<District> districts { get; private set; } = new List<District>();
    public List<POP> pops { get; private set; } = new List<POP>();
    public List<PlanetaryFeature> features { get; private set; } = new List<PlanetaryFeature>();

}

public enum PlanetaryFeature
{
    OilDeposit,
    ExtraordinaryOilDeposit,
    GoodQualityRockMount,
    IronVein,
    Grassland,
    FertileLand,
}
