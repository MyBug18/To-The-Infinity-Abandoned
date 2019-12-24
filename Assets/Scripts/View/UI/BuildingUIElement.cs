using UnityEngine;

public class BuildingUIElement : MonoBehaviour
{

    private Building building;

    public AuxiliaryUI auxiliaryUI;

    [SerializeField]
    private GameObject notYetBuiltUI, builtUI, upgradeButton;

    public bool isNotYetBuilt = true;

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
        if (isNotYetBuilt)
        {
            auxiliaryUI.gameObject.SetActive(true);
            auxiliaryUI.Initialize(AuxiliaryUIStatus.BuildingList);
        }
        else
        {

        }
    }

    public void OnConstructionFinished(Building b)
    {
        Debug.Log(b.name);
        building = b;

        notYetBuiltUI.SetActive(false);
        builtUI.SetActive(true);

        if (b is IUpgradable)
            upgradeButton.SetActive(true);
    }
}
