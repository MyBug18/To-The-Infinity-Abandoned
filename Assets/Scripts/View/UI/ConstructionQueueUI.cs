using UnityEngine;
using System.Collections.Generic;

public class ConstructionQueueUI : MonoBehaviour
{
    [SerializeField]
    private Transform elementPrefab;

    [SerializeField]
    private Transform queueContentsBody;

    [SerializeField]
    private ResourceUI resourceUI;

    [SerializeField]
    private BuildingUI buildingUI;

    [SerializeField]
    private DistrictUI districtUI;

    public ConstructionQueueUIElement PutElementOnQueue(Planet_Inhabitable planet)
    {
        ConstructionQueueUIElement elem = Instantiate(elementPrefab, queueContentsBody).GetComponent<ConstructionQueueUIElement>();
        elem.planet = planet;
        elem.constructionQueueUI = this;
        elem.resourceUI = resourceUI;
        elem.buildingUI = buildingUI;
        elem.districtUI = districtUI;

        return elem;
    }
}
