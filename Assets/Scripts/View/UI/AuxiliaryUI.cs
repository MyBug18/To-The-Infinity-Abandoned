using System;
using UnityEngine;

public class AuxiliaryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingListUI;

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

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

public enum AuxiliaryUIStatus
{
    Decision,
    BuildingList,
    BuildingStatus
}
