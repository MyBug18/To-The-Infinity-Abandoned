using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListUIElement : MonoBehaviour
{
    [SerializeField]
    private BuildingType type;

    [SerializeField]
    private Text costT, timeT;

    [SerializeField]
    private InhabitablePlanetUI inhabitablePlanetUI;

    [SerializeField]
    private BuildingUI buildingUI;

    [SerializeField]
    private ConstructionQueueUI constructionQueueUI;

    private Planet_Inhabitable planet;
    // Start is called before the first frame update
    void Start()
    {
        planet = inhabitablePlanetUI.planet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo()
    {
        costT.text = planet.workingPlaceFactory.GetConstructionCost(type).ToString();
        timeT.text = planet.workingPlaceFactory.GetConstructionTime(type).ToString();
    }

    
    public void OnClickBuildButton()
    {
        var cur = buildingUI.currentEmptyElement;

        Action onTimerEnded = () => cur.OnConstructionFinished(planet.workingPlaceFactory.GetBuilding(type));
        planet.StartConstruction(type, onTimerEnded);

        constructionQueueUI.PutElementOnQueue(planet);

        buildingUI.MakeNewBuildingElement();
    }
    
}
