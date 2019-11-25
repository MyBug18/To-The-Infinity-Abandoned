using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Inhabitable : Planet
{
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
    public System.Random r = new System.Random();

    public Planet_Inhabitable(string name, int size, Game game, StarOrbit orbit, int nthOrbit, bool isSatellite) : base(name, size, PlanetType.Inhabitable, game, orbit, nthOrbit, isSatellite) // for making special inhabitable planet, such as Earth.
    {
        System.Random r = new System.Random();
        for (int i = 0; i < size / 3 + 1; i++)
        {
            features.Add((PlanetaryFeature)(r.Next() % 6));
        }
    }

    public Planet_Inhabitable(string name, Game game, StarOrbit orbit, int nthOrbit, bool isSatellite) : base(name, game, orbit, nthOrbit, isSatellite, true) // for making general inhabitable planets.
    {
        System.Random r = new System.Random();
        for (int i = 0; i < size / 3 + 1; i++)
        {
            features.Add((PlanetaryFeature)(r.Next() % 6));
        }
    }

    public int maxBuildingSlotNum => 12;

    public int providedHousing = 0;
    public int providedAmenity = 0;

    public int consumedHousing = 0;
    public int consumedAmenity = 0;

    public int housing => providedHousing - consumedHousing;
    public int amenity => providedAmenity - consumedAmenity;

    private float _baseStability => pops.Count > 0 ? pops.Average(pop => pop.happiness) : 0;
    public float stabilityModifier = 0;
    public float stability => Math.Max(_baseStability + stabilityModifier, 0);

    private float _crimeByPOP => pops.Count;
    public float crimeReducedByEnforcer = 0;
    public float crime => Math.Max(0, _crimeByPOP - crimeReducedByEnforcer);

    public bool isColonized => pops.Count > 1;

    public List<Building> buildings { get; private set; } = new List<Building>();
    public List<District> districts { get; private set; } = new List<District>();
    public List<PlanetaryFeature> features { get; private set; } = new List<PlanetaryFeature>();

    public List<ConstructionQueueElement> ongoingConstruction = new List<ConstructionQueueElement>();

    public List<POP> pops = new List<POP>();
    public List<POP> unemployedPOPs = new List<POP>();
    public List<POP> trainingPOPs = new List<POP>();

    public List<JobUpkeep> planetJobUpkeeps = new List<JobUpkeep>();
    public List<WorkingPlaceBaseUpkeep> planetBaseUpkeeps = new List<WorkingPlaceBaseUpkeep>();
    public List<JobYield> planetJobYields = new List<JobYield>();

    public float currentPOPGrowth = 0;
    public float basePOPGrowth => pops.Count > 0 ? 5 : 0;
    public float foodPOPGrowthModifier => game.globalResource.isLackOfFood ? 0.25f : 1;
    public float POPGrowthRate { get
        {
            float baseRate = basePOPGrowth * foodPOPGrowthModifier;
            float result = baseRate * (1 + housingGrowthModifier);
            return result;
        }
    }

    public float additionalGrowthRate => housingGrowthModifier;
    public float housingGrowthModifier => Math.Min(0, (float)housing / pops.Count);

    public (int maxElectricity, int maxMineral, int maxFood) resourcesDistrictsMaxNum
    {
        get
        {
            int _electricity = 0, _mineral = 0, _food = 0;
            foreach (var f in features)
            {
                switch (f)
                {
                    case PlanetaryFeature.ExtraordinaryOilDeposit:
                        _electricity += 3;
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
                        _electricity += 2;
                        break;
                    default:
                        throw new InvalidOperationException("ERROR: Invalid Plenetary Feature Detected: " + f);
                }
            }

            return (_electricity, _mineral, _food);
        }
    }
    public int currentElectricityDistrictNum = 0, currentMineralDistrictNum = 0, currentFoodDistrictNum = 0, currentHouseDistrictNum = 0;
    public int availableElectricityDistrictNum => resourcesDistrictsMaxNum.maxElectricity - currentElectricityDistrictNum;
    public int availableMineralDistrictNum => resourcesDistrictsMaxNum.maxMineral - currentMineralDistrictNum;
    public int availableFoodDistrictNum => resourcesDistrictsMaxNum.maxFood - currentFoodDistrictNum;
    public int availableHouseDistrictNum => size - currentFoodDistrictNum - currentElectricityDistrictNum - currentMineralDistrictNum - currentHouseDistrictNum;

    public int remainColonizationDay { get; private set; } = 0;

    public void StartColonization()
    {
        remainColonizationDay = game.colonizationDate;
        game.ongoingColonization.Add(this);
    }

    public void DecreaseColonizationDay()
    {
        remainColonizationDay--;
    }

    public void EndColonization()
    {
        BirthPOP();
        _BuildBuilding(BuildingType.ColonizationCenter);
    }

    public void BirthPOP()
    {
        string[] str = System.IO.File.ReadAllLines(UnityEngine.Application.streamingAssetsPath + "\\name_list.txt");
        System.Random r = new System.Random();
        string popName = str[r.Next() % 1000];
        POP pop = new POP(str[r.Next() % 1000], this);

        consumedAmenity++;
        consumedHousing++;

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
            pop.currentWorkingSlot.pop = null;
        }

        if (pop.isTraining) // if training, remove from training pop list.
            trainingPOPs.Remove(pop);

        consumedAmenity--;
        consumedHousing--;

        pops.Remove(pop);
    }

    public bool IsDistrictBuildable(DistrictType type)
    {
        bool result;
        switch(type)
        {
            case DistrictType.Food:
                if (availableFoodDistrictNum <= 0) result = false;
                else result = true;
                break;
            case DistrictType.Fuel:
                if (availableElectricityDistrictNum <= 0) result = false;
                else result = true;
                break;
            case DistrictType.House:
                if (availableHouseDistrictNum <= 0) result = false;
                else result = true;
                break;
            case DistrictType.Mineral:
                if (availableMineralDistrictNum <= 0) result = false;
                else result = true;
                break;
            default: throw new InvalidOperationException("Invalid DistrictType detected!");
        }

        return result;
    }

    private void _BuildDistrict(DistrictType type)
    {
       districts.Add(WorkingPlaceFactory.GetDistrict(type, this));
    }

    private void _BuildBuilding(BuildingType type)
    {
        buildings.Add(WorkingPlaceFactory.GetBuilding(type, this));
    }

    public void StartConstruction(BuildingType type)
    {
        ongoingConstruction.Add(new ConstructionQueueElement(true, (int)type, WorkingPlaceFactory.GetConstructionTime(type), null));
    }

    public void StartConstruction(DistrictType type)
    {
        if (!IsDistrictBuildable(type)) throw new InvalidOperationException(type + " District is already at max number!");

        Debug.Log("StartConstruction");
        ongoingConstruction.Add(new ConstructionQueueElement(false, (int)type, WorkingPlaceFactory.GetConstructionTime(type), null));
    }

    public void StartUpgrade(Building fromUpgrade)
    {
        if (!((IUpgradable)fromUpgrade).IsUpgradable()) throw new InvalidOperationException("This building has not met the upgrade condition!");
        if (!(fromUpgrade is IUpgradable)) throw new InvalidOperationException("This building is not upgradable!");

        ongoingConstruction.Add(new ConstructionQueueElement(true, (int)(fromUpgrade.type + 1), 
            WorkingPlaceFactory.GetConstructionTime((BuildingType)((int)(fromUpgrade.type) + 1)), fromUpgrade)); // Every upgraded BuildingType is bigger by 1 than a previous one.
    }

    public void ProceedConstruction()
    {
        if (ongoingConstruction.Count > 0)
        {
            if (ongoingConstruction[0].remainTime > 0)
            {
                ongoingConstruction[0].remainTime--;
                Debug.Log(ongoingConstruction[0].remainTime);
            }
            else
            {
                EndConstruction();
            }
        }
    }

    public void EndConstruction()
    {
        ConstructionQueueElement justEnded = ongoingConstruction[0];
        ongoingConstruction.RemoveAt(0);

        if (justEnded.isBuilding == false)
        {
            _BuildDistrict((DistrictType)(justEnded.type));
        }
        else
        {
            if (justEnded.fromUpgrade == null)
                buildings.Add(WorkingPlaceFactory.GetBuilding((BuildingType)justEnded.type, this));
            else
                ((IUpgradable)justEnded.fromUpgrade).Upgrade();
        }
        Debug.Log("Construction Ended");
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

    public override string ToString()
    {
        string basic = base.ToString();
        string values = "Housing : " + housing + ", Amenity: " + amenity + ", Crime: " + crime + ", Stability: " + stability + ", POP growth rate: " + POPGrowthRate + "\n";

        string _districts = "Districts:\n";
        foreach (var d in districts)
            _districts += d.name + "\n";

        string _buildings = "Buildings:\n";
        foreach (var b in buildings)
            _buildings += b.name + "\n";

        string _currentConstructing = "Currently constructing: ";
        foreach (var q in ongoingConstruction)
        {
            if (q.isBuilding)
                _currentConstructing += (BuildingType)(q.type);
            else
                _currentConstructing += (DistrictType)(q.type);
        }
        return basic + values + _districts + _buildings + _currentConstructing;
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
