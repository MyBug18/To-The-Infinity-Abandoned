using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DistrictUIManager : MonoBehaviour
{
    public Planet_Inhabitable planet;

    [SerializeField]
    private Text housingNum, electricityNum, mineralNum, foodNum;

    [SerializeField]
    private Transform housingSqrs, electricitySqrs, mineralSqrs, foodSqrs;

    [SerializeField]
    private Button housePlus, elecPlus, mineralPlus, foodPlus;

    [SerializeField]
    private ConstructionQueueUI constructionQueue;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        _InitializeTexts();
        _InitializeSqrs();
        _EnableOrDisableButton();
    }

    private void _InitializeTexts()
    {
        int buildedH = planet.currentHouseDistrictNum;
        int maxH = planet.availableHouseDistrictNum;
        housingNum.text = buildedH + "/" + maxH;

        int buildedE = planet.currentElectricityDistrictNum;
        int maxE = planet.availableElectricityDistrictNum;
        electricityNum.text = buildedE + "/" + maxE;

        int buildedM = planet.currentMineralDistrictNum;
        int maxM = planet.availableMineralDistrictNum;
        mineralNum.text = buildedM + "/" + maxM;

        int buildedF = planet.currentFoodDistrictNum;
        int maxF = planet.availableFoodDistrictNum;
        foodNum.text = buildedF + "/" + maxF;
    }

    private void _InitializeSqrs()
    {
        _InitializeGraySqrs(housingSqrs, planet.availableHouseDistrictNum);
        _InitializeGraySqrs(electricitySqrs, planet.availableElectricityDistrictNum);
        _InitializeGraySqrs(mineralSqrs, planet.availableMineralDistrictNum);
        _InitializeGraySqrs(foodSqrs, planet.availableFoodDistrictNum);
    }

    private void _InitializeGraySqrs(Transform sqrs, int max)
    {
        if (max >= 12) return;

        for (int i = max; i < 12; i++)
            sqrs.GetChild(i).gameObject.SetActive(false);
    }

    public void BuildDistrict(int a)
    {
        DistrictType type = (DistrictType)a;

        Action OnTimerEnded = () => _AddSquare(type);
        planet.StartConstruction(type, OnTimerEnded);

        _EnableOrDisableButton();

        switch (type)
        {
            case DistrictType.Electricity:
                electricityNum.text = (planet.currentElectricityDistrictNum + planet.plannedElectricityDistrictNum) + "/" + planet.resourcesDistrictsMaxNum.maxElectricity;
                housingNum.text = (planet.currentHouseDistrictNum + planet.plannedHouseDistrictNum) + "/" + planet.availableHouseDistrictNum;
                break;
            case DistrictType.Mineral:
                mineralNum.text = (planet.currentMineralDistrictNum + planet.plannedMineralDistrictNum) + "/" + planet.resourcesDistrictsMaxNum.maxMineral;
                housingNum.text = (planet.currentHouseDistrictNum + planet.plannedHouseDistrictNum) + "/" + planet.availableHouseDistrictNum;
                break;
            case DistrictType.Food:
                foodNum.text = (planet.currentFoodDistrictNum + planet.plannedFoodDistrictNum) + "/" + planet.resourcesDistrictsMaxNum.maxFood;
                housingNum.text = (planet.currentHouseDistrictNum + planet.plannedHouseDistrictNum) + "/" + planet.availableHouseDistrictNum;
                break;
            case DistrictType.House:
                housingNum.text = (planet.currentHouseDistrictNum + planet.plannedHouseDistrictNum) + "/" + planet.availableHouseDistrictNum;
                break;
        }

        constructionQueue.PutElementOnQueue(planet);
    }

    private void _AddSquare(DistrictType type)
    {
        switch(type)
        {
            case DistrictType.House:
                housingSqrs.GetChild(planet.currentHouseDistrictNum - 1).GetChild(0).gameObject.SetActive(true);
                break;
            case DistrictType.Electricity:
                electricitySqrs.GetChild(planet.currentElectricityDistrictNum - 1).GetChild(0).gameObject.SetActive(true);
                break;
            case DistrictType.Mineral:
                mineralSqrs.GetChild(planet.currentMineralDistrictNum - 1).GetChild(0).gameObject.SetActive(true);
                break;
            case DistrictType.Food:
                foodSqrs.GetChild(planet.currentFoodDistrictNum - 1).GetChild(0).gameObject.SetActive(true);
                break;
            default:
                throw new InvalidOperationException("Invalid District: " + type);
        }
    }

    private void _EnableOrDisableButton() 
    { 
        if (planet.IsDistrictBuildable(DistrictType.Electricity)) elecPlus.interactable = true; else elecPlus.interactable = false;
        if (planet.IsDistrictBuildable(DistrictType.Mineral)) mineralPlus.interactable = true; else mineralPlus.interactable = false;
        if (planet.IsDistrictBuildable(DistrictType.Food)) foodPlus.interactable = true; else foodPlus.interactable = false;
        if (planet.IsDistrictBuildable(DistrictType.House)) housePlus.interactable = true; else housePlus.interactable = false;
    }
}
