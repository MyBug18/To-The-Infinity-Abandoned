using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField]
    private Text electricityText, mineralText, foodText, alloyText, moneyText, researchText;

    public Planet_Inhabitable planet;
    private Resource resourceInfo;

    // Start is called before the first frame update
    public void Initialize()
    {
        resourceInfo = planet.planetaryResources;
        planet.game.MonthEvents += () => UpdateResourceUI();
        UpdateResourceUI();
    }

    private string _MinusOrPlus(float v)
    {
        if (v < 0) return "(" + v.ToString("0.00") + ")";
        else return "(+" + v.ToString("0.00") + ")";
    }

    public void UpdateResourceUI()
    {
        Debug.Log("asdf");
        electricityText.text = resourceInfo.electricity.ToString("0.00") + " " + _MinusOrPlus(resourceInfo.turnResource.turnElectricity);
        mineralText.text = resourceInfo.mineral.ToString("0.00") + " " + _MinusOrPlus(resourceInfo.turnResource.turnMineral);
        foodText.text = resourceInfo.food.ToString("0.00") + " " + _MinusOrPlus(resourceInfo.turnResource.turnFood);
        alloyText.text = resourceInfo.alloy.ToString("0.00") + " " + _MinusOrPlus(resourceInfo.turnResource.turnAlloy);
        moneyText.text = resourceInfo.money.ToString("0.00") + " " + _MinusOrPlus(resourceInfo.turnResource.turnMoney);
        researchText.text = "+" + (resourceInfo.turnResource.turnEngineering + resourceInfo.turnResource.turnPhysics + resourceInfo.turnResource.turnSociology).ToString("0.00");
    }
}
