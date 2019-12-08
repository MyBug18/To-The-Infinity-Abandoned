using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistrictUIManager : MonoBehaviour
{
    public Planet_Inhabitable planet;

    [SerializeField]
    private Text housingNum, electricityNum, mineralNum, foodNum;

    [SerializeField]
    private Transform housingSqrs, electricitySqrs, mineralSqrs, foodSqrs;


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
}
