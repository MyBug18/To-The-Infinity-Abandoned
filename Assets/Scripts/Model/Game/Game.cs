using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Game
{
    public int year { get; private set; } = 2100;
    public int month { get; private set; } = 1;
    public int day { get; private set; } = 1;
    public string date => year + "." + month + "." + day;

    public List<Planet_Inhabitable> colonizedPlanets = new List<Planet_Inhabitable>();
    public List<Planet_Inhabitable> ongoingColonization = new List<Planet_Inhabitable>();

    private float _baseColonizationDate = 7;
    private float _colonizationDateModifier = 1;

    public int colonizationDate => (int)(_baseColonizationDate * _colonizationDateModifier);

    public float taxRate = 1;

    public int fleetNum = 1;
    public float defencePlatformAttackModifier = 1;
    public float fleetAttackModifier = 1;

    public float constructionTimeModifier = 10;
    public float constructionCostModifier = 1;

    public float miningStationModifier = 1;
    public float researchStationModifier = 1;

    private List<string> _unusedStarName;
    private List<string> _unusedBlackholeName;

    public List<StarSystem> systems { get; private set; } = new List<StarSystem>();

    public event Action DayEvents, MonthEvents, YearEvents;
    public Game()
    {
        _unusedStarName = File.ReadAllLines(UnityEngine.Application.streamingAssetsPath + "\\star_name_list.txt").ToList();
        _unusedBlackholeName = File.ReadAllLines(UnityEngine.Application.streamingAssetsPath + "\\blackhole_name_list.txt").ToList();
    }

    public string GetStarSystemName()
    {
        int index = GameDataHolder.random.Next() % _unusedStarName.Count;
        string result = _unusedStarName[index];
        _unusedStarName.RemoveAt(index);
        return result;
    }

    public string GetBlackholeName()
    {
        int index = GameDataHolder.random.Next() % _unusedBlackholeName.Count;
        string result = _unusedBlackholeName[index];
        _unusedBlackholeName.RemoveAt(index);
        return result;
    }

    public void AddRandomStarSystem()
    {
        systems.Add(new StarSystem(this));
    }

    public void IncreaseOneDay()
    {
        int _lastDayOfMonth;

        switch(month)
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                _lastDayOfMonth = 31;
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                _lastDayOfMonth = 30;
                break;
            case 2:
                if (year % 100 != 0 && year % 4 == 0)
                    _lastDayOfMonth = 29;
                else
                    _lastDayOfMonth = 28;
                break;

            default:
                throw new InvalidOperationException("ERROR: " + month + "th month appeared!");
        }

        if (day < _lastDayOfMonth)
        {
            day++;
        }
        else
        {
            day = 1;
            _IncreaseOneMonth();
        }

        _OneDayEvents();
    }

    private void _IncreaseOneMonth()
    {

        if (month < 12)
        {
            month++;
        }
        else
        {
            month = 1;
            _IncreaseOneYear();
        }

        _OneMonthEvents();
    }

    private void _IncreaseOneYear()
    {
        year++;

        _OneYearEvents();
    }

    private void _OneDayEvents()
    {
        _ProceedTraining();
        _ProceedColonization();
        _ProceedPlanetaryConstruction();

        DayEvents?.Invoke();
    }

    private void _OneMonthEvents()
    {
        _ProceedPlanetaryGrowth();
        foreach (var p in colonizedPlanets)
        {
            p.planetaryResources.ApplyTurnResource();
        }

        MonthEvents?.Invoke();
    }

    private void _OneYearEvents()
    {
        YearEvents?.Invoke();
    }

    private void _ProceedPlanetaryGrowth()
    {
        foreach (var planet in colonizedPlanets)
            planet.ProceedGrowth();
    }

    private void _ProceedTraining() // Should be called with IncreaseOneDay()
    {
        List<POP> toRemoveFromTrainingList = new List<POP>();
        foreach (var planet in colonizedPlanets)
        {
            foreach (var pop in planet.trainingPOPs)
            {
                if (pop.remainTrainingDay > 0)
                    pop.DecreaseTrainingDay();
                else
                {
                    pop.EndTraining();
                    toRemoveFromTrainingList.Add(pop);
                }
            }

            foreach (var pop in toRemoveFromTrainingList)
                planet.trainingPOPs.Remove(pop);
        }
    }    

    private void _ProceedColonization()
    {
        List<Planet_Inhabitable> toRemoveFromColonizationList = new List<Planet_Inhabitable>();
        foreach (var planet in ongoingColonization)
        {
            if (planet.remainColonizationDay > 0)
                planet.DecreaseColonizationDay();
            else
            {
                planet.EndColonization();
                colonizedPlanets.Add(planet);
                toRemoveFromColonizationList.Add(planet);
            }
        }

        foreach (var planet in toRemoveFromColonizationList)
            ongoingColonization.Remove(planet);
    }

    private void _ProceedPlanetaryConstruction()
    {
        foreach (var planet in colonizedPlanets)
        {
            planet.ProceedConstruction();
        }
    }

    public void AddColonizationSpeedModifier(float v)
    {
        _colonizationDateModifier += v;
    }
}


