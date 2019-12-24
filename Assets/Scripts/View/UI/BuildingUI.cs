using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buildingContent, buildingElementPrefab;

    [SerializeField]
    private AuxiliaryUI auxiliaryUI;

    [SerializeField]
    private ConstructionQueueUI constructionQueueUI;

    [SerializeField]
    private ResourceUI resourceUI;

    public List<BuildingUIElement> elements = new List<BuildingUIElement>();

    public BuildingUIElement currentEmptyElement => elements[elements.Count - 1];

    void Start()
    {
        MakeNewBuildingElement();
    }

    public void MakeNewBuildingElement()
    {
        var elem = Instantiate(buildingElementPrefab, buildingContent).GetComponent<BuildingUIElement>();
        elem.auxiliaryUI = auxiliaryUI;
        elem.constructionQueueUI = constructionQueueUI;
        elem.resourceUI = resourceUI;
        elements.Add(elem);
    }
}
