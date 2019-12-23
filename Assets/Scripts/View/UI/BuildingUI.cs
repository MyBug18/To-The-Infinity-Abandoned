using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buildingContent, buildingElementPrefab;

    [SerializeField]
    private AuxiliaryUI auxiliaryUI;

    public List<BuildingUIElement> elements = new List<BuildingUIElement>();

    public BuildingUIElement currentEmptyElement => elements[elements.Count - 1];

    // Start is called before the first frame update
    void Start()
    {
        MakeNewBuildingElement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNewBuildingElement()
    {
        var elem = Instantiate(buildingElementPrefab, buildingContent).GetComponent<BuildingUIElement>();
        elem.auxiliaryUI = auxiliaryUI;
        elements.Add(elem);
    }
}
