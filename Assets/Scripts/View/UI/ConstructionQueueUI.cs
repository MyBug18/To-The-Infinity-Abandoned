using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionQueueUI : MonoBehaviour
{
    [SerializeField]
    private Transform elementPrefab;
    
    public void PutElementOnQueue(Planet_Inhabitable planet)
    {
        Transform elem = Instantiate(elementPrefab, transform);
        elem.GetComponent<ConstructionQueueElementUI>().planet = planet;
    }
}
