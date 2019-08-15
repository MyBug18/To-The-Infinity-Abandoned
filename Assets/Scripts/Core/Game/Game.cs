using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Game
{
    public static int year { get; private set; } = 2100;
    public static int month { get; private set; } = 1;
    public static int day { get; private set; } = 1;
    public static string date => year + "." + month + "." + day;
    public static Resource globalResource = new Resource();
    public static List<Planet_Inhabitable> planets = new List<Planet_Inhabitable>();
    

    public static float taxRate;
    public static float popFoodUpkeepRate;

    public static void IncreaseOneDay()
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
                if (year % 4 == 0)
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

        _ProceedTraining();
    }

    private static void _IncreaseOneMonth()
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
    }

    private static void _IncreaseOneYear()
    {
        year++;
    }




    
    private static void _ProceedTraining() // Should be called with IncreaseOneDay()
    {
        List<POP> toRemoveFromTrainingList = new List<POP>();
        foreach (var planet in planets)
        {
            foreach (var pop in planet.trainingPOPs)
            {
                if (pop.remainTrainingDay > 0)
                    pop.DecreaseTrainingDay();
                else
                {
                    pop.MoveJob();
                    toRemoveFromTrainingList.Add(pop);
                }
            }

            foreach (var pop in toRemoveFromTrainingList)
                planet.trainingPOPs.Remove(pop);
        }
    }
    

}


