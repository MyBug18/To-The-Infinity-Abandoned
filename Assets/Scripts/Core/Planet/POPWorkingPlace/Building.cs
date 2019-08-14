using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Building
{
    public POP[] workingPOP { get; private set; }
    public BuildingType buildingType { get; private set; }

    public float baseFuelUpkeep { get; private set; }
}

public enum BuildingType
{
    AlloyFoundry,
    Lab,
    PlanetaryMarket,
    GovernmentHall,
    PoliceStation,
    ControlTower,
    FleetTrainingCenter,
    CommandCenter,
    MineralExtractor,
    BioPlant
}
