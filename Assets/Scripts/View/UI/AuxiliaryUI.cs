using System;
using UnityEngine;

public class AuxiliaryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingListUI;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void Initialize(AuxiliaryUIStatus status)
    {
        switch (status)
        {
            case AuxiliaryUIStatus.BuildingList:
                buildingListUI.SetActive(true);
                break;
            default:
                throw new InvalidOperationException("Invalid enum!");
        }
    }
}

public enum AuxiliaryUIStatus
{
    Decision,
    BuildingList,
    BuildingStatus
}
