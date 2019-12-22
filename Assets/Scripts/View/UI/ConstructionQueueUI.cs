using UnityEngine;

public class ConstructionQueueUI : MonoBehaviour
{
    [SerializeField]
    private Transform elementPrefab;

    [SerializeField]
    private Transform queueContentsBody;
    
    public void PutElementOnQueue(Planet_Inhabitable planet)
    {
        Transform elem = Instantiate(elementPrefab, queueContentsBody);
        elem.GetComponent<ConstructionQueueUIElement>().planet = planet;
    }
}
