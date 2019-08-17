﻿using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Building : POPWorkingPlace
{
    public Building(Planet_Inhabitable planet) : base(WorkingPlaceType.Building, planet) { }

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