using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Inhabitable : Planet
{

    public Planet_Inhabitable(string name, int size, Game game) : base(name, size, PlanetType.Inhabitable, game) { }

    public int maxBuildingSlotNum => 12;

    public int housing { get; private set; } = 0;
    public float stability { get; private set; } = 50;
    public float crime { get; private set; } = 0;
    public int amenity { get; private set; } = 0;

    public bool isColonized => pops.Count > 1;

    public List<Building> buildings { get; private set; } = new List<Building>();
    public List<District> districts { get; private set; } = new List<District>();
    public List<PlanetaryFeature> features { get; private set; } = new List<PlanetaryFeature>();

    public List<POP> pops = new List<POP>();
    public List<POP> unemployedPOPs = new List<POP>();
    public List<POP> trainingPOPs = new List<POP>();

    public List<JobUpkeep> planetJobUpkeeps = new List<JobUpkeep>();
    public List<WorkingPlaceBaseUpkeep> planetBaseUpkeeps = new List<WorkingPlaceBaseUpkeep>();

    public List<JobYield> planetJobYields = new List<JobYield>();

    private float _basePOPGrowth => 5;
    private float _POPGrowthModifier = 1;
    public float POPGrowthRate => _basePOPGrowth * _POPGrowthModifier;

    public int remainColonizationDay { get; private set; } = 0;

    public void StartColonization()
    {
        Debug.Log("Start Colonization");

        remainColonizationDay = game.colonizationDate;
        game.ongoingColonization.Add(this);
    }

    public void DecreaseColonizationDay()
    {
        remainColonizationDay--;
    }

    public void EndColonization()
    {
        Debug.Log("Colonization Ended");
        BirthPOP();
        // build planetary capital.
    }

    public void BirthPOP()
    {
        string[] str = System.IO.File.ReadAllText("Assets\\Scripts\\Core\\Planet\\namelist.txt").Split('\n');
        System.Random r = new System.Random();
        string popName = str[r.Next() % 1000];

        var pop = new POP(str[r.Next() % 1000], this);
        pops.Add(pop);
        unemployedPOPs.Add(pop);
    }

    public (int maxFuel, int maxMineral, int maxFood) resourcesDistrictsMaxNum { get {
            int _fuel = 0, _mineral = 0, _food = 0;
            foreach(var f in features)
            {
                switch(f)
                {
                    case PlanetaryFeature.ExtraordinaryOilDeposit:
                        _fuel += 3;
                        break;
                    case PlanetaryFeature.FertileLand:
                        _food += 3;
                        break;
                    case PlanetaryFeature.GoodQualityRockMount:
                        _mineral += 2;
                        break;
                    case PlanetaryFeature.Grassland:
                        _food += 2;
                        break;
                    case PlanetaryFeature.IronVein:
                        _mineral += 3;
                        break;
                    case PlanetaryFeature.OilDeposit:
                        _fuel += 2;
                        break;
                    default:
                        throw new InvalidOperationException("ERROR: Invalid Plenetary Feature Detected: " + f);
                }
            }

            return (_fuel, _mineral, _food);
        }
    }
    public int currentFuelDistrictNum = 0, currentMineralDistrictNum = 0, currentFoodDistrictNum = 0, currentHouseDistrictNum = 0;
    public int availableHouseDistrictNum => size - currentFoodDistrictNum - currentFuelDistrictNum - currentMineralDistrictNum - currentHouseDistrictNum;

    public void BuildDistrict(DistrictType type)
    {
        districts.Add(WorkingPlaceFactory.BuildDistrict(type, this));

        switch(type)
        {
            case DistrictType.Food:
                currentFoodDistrictNum++;
                break;
            case DistrictType.Fuel:
                currentFuelDistrictNum++;
                break;
            case DistrictType.Housing:
                currentHouseDistrictNum++;
                break;
            case DistrictType.Mineral:
                currentMineralDistrictNum++;
                break;
        }
    }
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
