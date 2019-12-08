using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionQueueUI : MonoBehaviour
{
    [SerializeField]
    private Transform elementPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutElementOnQueue(Planet_Inhabitable planet)
    {
        Transform elem = Instantiate(elementPrefab, transform);
        elem.GetComponent<ConstructionQueueElementUI>().planet = planet;
    }
}
