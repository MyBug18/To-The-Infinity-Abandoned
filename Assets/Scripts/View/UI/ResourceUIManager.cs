using UnityEngine;
using UnityEngine.UI;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField]
    private Text electricityText, mineralText, foodText, alloyText, moneyText, researchText;

    public Planet_Inhabitable planet;
    private Resource resourceInfo;

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
        electricityText.text = processFloat(resourceInfo.electricity) + "\n" + _MinusOrPlus(resourceInfo.turnResource.turnElectricity);
        mineralText.text = processFloat(resourceInfo.mineral) + "\n" + _MinusOrPlus(resourceInfo.turnResource.turnMineral);
        foodText.text = processFloat(resourceInfo.food) + "\n" + _MinusOrPlus(resourceInfo.turnResource.turnFood);
        alloyText.text = processFloat(resourceInfo.alloy) + "\n" + _MinusOrPlus(resourceInfo.turnResource.turnAlloy);
        moneyText.text = processFloat(resourceInfo.money) + "\n" + _MinusOrPlus(resourceInfo.turnResource.turnMoney);
        researchText.text = "+" + processFloat(resourceInfo.turnResource.turnEngineering + resourceInfo.turnResource.turnPhysics + resourceInfo.turnResource.turnSociology);
    }

    private string processFloat(float value)
    {
        if (value > 1000) return (value / 1000) + "k";
        else return value.ToString();
    }
}
