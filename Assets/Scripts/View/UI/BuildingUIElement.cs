using UnityEngine;
using System;

public class BuildingUIElement : MonoBehaviour
{

    private Building building;

    public AuxiliaryUI auxiliaryUI;
    public ConstructionQueueUI constructionQueueUI;
    public ResourceUI resourceUI;

    [SerializeField]
    private GameObject notYetBuiltUI, builtUI, upgradeButton, currentlyBuildingUI, UpgradingUI;

    public BuildingUIElementStatus status = BuildingUIElementStatus.NotBuilt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (status == BuildingUIElementStatus.NotBuilt)
        {
            auxiliaryUI.gameObject.SetActive(true);
            auxiliaryUI.Initialize(AuxiliaryUIStatus.BuildingList);
        }
        else
        {
            Debug.Log(building.name);
        }
    }

    public void OnConstructionFinished(Building b)
    {
        building = b;
        ChangeStatus(BuildingUIElementStatus.AlreadyBuilt);

        if (b is IUpgradable)
            ActivateUpgradeButton();
    }

    public void OnUpgradeFinished()
    {
        ChangeStatus(BuildingUIElementStatus.AlreadyBuilt);
    }

    public void OnClickUpgrade()
    {
        ChangeStatus(BuildingUIElementStatus.Upgrading);
        Action onTimerEnded = () => OnUpgradeFinished();
        building.planet.StartUpgrade(building, onTimerEnded);
        upgradeButton.SetActive(false);

        var elem = constructionQueueUI.PutElementOnQueue(building.planet);

        elem.targetBuildingSlot = this;
        resourceUI.UpdateResourceUI();
    }

    public void ChangeStatus(BuildingUIElementStatus status)
    {
        this.status = status;
        notYetBuiltUI.SetActive(false);
        currentlyBuildingUI.SetActive(false);
        builtUI.SetActive(false);
        UpgradingUI.SetActive(false);

        switch(status)
        {
            case BuildingUIElementStatus.AlreadyBuilt:
                builtUI.SetActive(true);
                break;
            case BuildingUIElementStatus.CurrentlyBuilding:
                currentlyBuildingUI.SetActive(true);
                break;
            case BuildingUIElementStatus.NotBuilt:
                notYetBuiltUI.SetActive(true);
                break;
            case BuildingUIElementStatus.Upgrading:
                UpgradingUI.SetActive(true);
                break;
            default:
                throw new InvalidOperationException("BuildingUIElementStatus enum invalid!");
        }
    }

    public void ActivateUpgradeButton()
    {
        upgradeButton.SetActive(true);
    }
}

public enum BuildingUIElementStatus
{
    NotBuilt,
    CurrentlyBuilding,
    AlreadyBuilt,
    Upgrading
}