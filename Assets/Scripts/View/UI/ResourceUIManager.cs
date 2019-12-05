using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private Text date, electricityText, mineralText, foodText, alloyText, moneyText, researchText;

    private Game _game;
    private Resource _resourceInfo;

    // Start is called before the first frame update
    void Start()
    {
        _game = gm.game;
        _resourceInfo = _game.globalResource;
        _game.DayEvents += () => { date.text = _game.date; };
        _game.MonthEvents += _UpdateResourceInfo;
        _UpdateResourceInfo();
    }

    private string _MinusOrPlus(float v)
    {
        if (v < 0) return "(" + v.ToString("0.00") + ")";
        else return "(+" + v.ToString("0.00") + ")";
    }

    private void _UpdateResourceInfo()
    {
        electricityText.text = _resourceInfo.electricity.ToString("0.00") + " " + _MinusOrPlus(_resourceInfo.turnResource.turnElectricity);
        mineralText.text = _resourceInfo.mineral.ToString("0.00") + " " + _MinusOrPlus(_resourceInfo.turnResource.turnMineral);
        foodText.text = _resourceInfo.food.ToString("0.00") + " " + _MinusOrPlus(_resourceInfo.turnResource.turnFood);
        alloyText.text = _resourceInfo.alloy.ToString("0.00") + " " + _MinusOrPlus(_resourceInfo.turnResource.turnAlloy);
        moneyText.text = _resourceInfo.money.ToString("0.00") + " " + _MinusOrPlus(_resourceInfo.turnResource.turnMoney);
        researchText.text = "+" + (_resourceInfo.turnResource.turnEngineering + _resourceInfo.turnResource.turnPhysics + _resourceInfo.turnResource.turnSociology).ToString("0.00");
    }
}
