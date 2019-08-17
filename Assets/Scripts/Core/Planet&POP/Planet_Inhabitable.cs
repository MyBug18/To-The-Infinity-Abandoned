using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Inhabitable : Planet
{

    public Planet_Inhabitable(string name, int size, Game game) : base(name, size, PlanetType.Inhabitable, game)
    {
        System.Random r = new System.Random();
        for (int i = 0; i < size / 3 + 1; i++)
        {
            features.Add((PlanetaryFeature)(r.Next() % 6));
        }
    }

    public int maxBuildingSlotNum => 12;

    public int housing = 0;
    public float stability = 50;
    public float crime = 0;
    public int amenity = 0;

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

    public float currentPOPGrowth = 0;
    private float _basePOPGrowth => 56;
    public float POPGrowthModifier = 1;
    public float POPGrowthRate => _basePOPGrowth * POPGrowthModifier;

    public (int maxFuel, int maxMineral, int maxFood) resourcesDistrictsMaxNum
    {
        get
        {
            int _fuel = 0, _mineral = 0, _food = 0;
            foreach (var f in features)
            {
                switch (f)
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
    public int availableFuelDistrictNum => resourcesDistrictsMaxNum.maxFuel - currentFuelDistrictNum;
    public int availableMineralDistrictNum => resourcesDistrictsMaxNum.maxMineral - currentMineralDistrictNum;
    public int availableFoodDistrictNum => resourcesDistrictsMaxNum.maxFood - currentFoodDistrictNum;
    public int availableHouseDistrictNum => size - currentFoodDistrictNum - currentFuelDistrictNum - currentMineralDistrictNum - currentHouseDistrictNum;

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
        string[] str = System.IO.File.ReadAllText("Assets\\Scripts\\Core\\Planet&POP\\namelist.txt").Split('\n');
        System.Random r = new System.Random();
        string popName = str[r.Next() % 1000];

        var pop = new POP(str[r.Next() % 1000], this);
        pops.Add(pop);
        unemployedPOPs.Add(pop);
    }

    public void KillPOP(POP pop)
    {
        Debug.Log("Killing: " + pop);

        if (pop.isUnemployed) // if unemployed, remove from unemployed pop list.
            unemployedPOPs.Remove(pop);
        else
        {
            var workingSlot = pop.currentWorkingPlace.workingPlace.workingPOPList[pop.currentWorkingPlace.slotNum];
            workingSlot.pop = null;
        }

        if (pop.isTraining) // if training, remove from training pop list.
            trainingPOPs.Remove(pop);

        pops.Remove(pop);

    }

    public void BuildDistrict(DistrictType type)
    {
        switch (type)
        {
            case DistrictType.Food:
                if (currentFoodDistrictNum >= resourcesDistrictsMaxNum.maxFood) throw new InvalidOperationException("The number of Food District is at max!");
                currentFoodDistrictNum++;
                districts.Add(WorkingPlaceFactory.BuildDistrict(type, this));
                break;
            case DistrictType.Fuel:
                if (currentFuelDistrictNum >= resourcesDistrictsMaxNum.maxFuel) throw new InvalidOperationException("The number of Fuel District is at max!");
                currentFuelDistrictNum++;
                districts.Add(WorkingPlaceFactory.BuildDistrict(type, this));
                break;
            case DistrictType.Housing:
                if (currentHouseDistrictNum >= resourcesDistrictsMaxNum.maxFood) throw new InvalidOperationException("The number of House District is at max!");
                currentHouseDistrictNum++;
                districts.Add(WorkingPlaceFactory.BuildDistrict(type, this));
                break;
            case DistrictType.Mineral:
                if (currentMineralDistrictNum >= resourcesDistrictsMaxNum.maxMineral) throw new InvalidOperationException("The number of Mineral District is at max!");
                currentMineralDistrictNum++;
                districts.Add(WorkingPlaceFactory.BuildDistrict(type, this));
                break;
        }
    }

    public void DemolishWorkingPlace(POPWorkingPlace workingPlace)
    {
        if (!workingPlace.IsDemolishable()) throw new InvalidOperationException("There is a POP occupying a slot!");
        else
        {
            workingPlace.OnDemolishing();
            if (workingPlace is District)
            {
                districts.Remove((District)workingPlace);
            }

            if (workingPlace is Building)
            {
                buildings.Remove((Building)workingPlace);
            }
        }
    }

    public void ProceedGrowth()
    {
        currentPOPGrowth += POPGrowthRate;
        if (currentPOPGrowth >= 100)
        {
            currentPOPGrowth -= 100;
            BirthPOP();
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
