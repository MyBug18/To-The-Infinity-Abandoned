using UnityEngine;
using UnityEngine.UI;

public class ConstructionQueueUIElement : MonoBehaviour
{
    public Planet_Inhabitable planet;
    private ConstructionQueueElement element;

    [SerializeField]
    private Text placeName, remainingTime;

    public ConstructionQueueUI constructionQueueUI;
    public ResourceUI resourceUI;
    public DistrictUI districtUI;
    public BuildingUI buildingUI;

    public BuildingUIElement targetBuildingSlot = null;

    void Start()
    {
        element = planet.ongoingConstruction[planet.ongoingConstruction.Count - 1];

        placeName.text = element.isBuilding ? ((BuildingType)element.type).ToString() : ((DistrictType)element.type).ToString();
        remainingTime.text = element.remainTime.ToString();

        planet.game.DayEvents += () => { remainingTime.text = element.remainTime.ToString(); };
        element.OnTimerEnded += () => { Destroy(gameObject); };
    }

    public void OnClickRemoveButton()
    {
        _RecoverToBeforeConstruction();
        planet.CancelFromConstructionQueue(element.index);
        Destroy(gameObject);
    }

    private void _RecoverToBeforeConstruction()
    {
        if (!element.isBuilding)
            districtUI.UpdateButtonStatus();
        else
        {
            if (element.fromUpgrade == null)
            {
                buildingUI.elements.Remove(targetBuildingSlot);
                Destroy(targetBuildingSlot.gameObject);
            }
            else
            {
                targetBuildingSlot.ChangeStatus(BuildingUIElementStatus.AlreadyBuilt);
                targetBuildingSlot.ActivateUpgradeButton();
            }
        }
    }
}
