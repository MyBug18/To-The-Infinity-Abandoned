using UnityEngine;
using UnityEngine.UI;

public class ConstructionQueueUIElement : MonoBehaviour
{
    public Planet_Inhabitable planet;

    [SerializeField]
    private Text placeName, remainingTime;

    private ConstructionQueueElement element;

    void Start()
    {
        element = planet.ongoingConstruction[planet.ongoingConstruction.Count - 1];

        placeName.text = element.isBuilding ? ((BuildingType)element.type).ToString() : ((DistrictType)element.type).ToString();
        remainingTime.text = element.remainTime.ToString();

        planet.game.DayEvents += () => { remainingTime.text = element.remainTime.ToString(); };
        element.OnTimerEnded += () => { Destroy(gameObject); };
    }
}
