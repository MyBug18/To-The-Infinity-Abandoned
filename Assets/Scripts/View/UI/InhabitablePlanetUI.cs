using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InhabitablePlanetUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    public Planet_Inhabitable planet { private get; set; }

    [SerializeField]
    private Text planetNameT, planetSizeT, stabilityT, crimeT, popT, remainingJobSlotT, amenityT, housingT, unemployedPopT, averageHappinessT;

    [SerializeField]
    private DistrictUIManager districtUIManager;

    [SerializeField]
    private ResourceUIManager resourceUIManager;

    private void Start()
    {
        gm.game.DayEvents += () => UpdatePlanetUI();
    }

    void OnEnable()
    {
        _Initiallize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void _Initiallize()
    {
        planetNameT.text = planet.name;
        planetSizeT.text = planet.size.ToString();

        UpdatePlanetUI();

        districtUIManager.planet = planet;
        districtUIManager.Initialize();

        resourceUIManager.planet = planet;
        resourceUIManager.Initialize();

    }

    public void UpdatePlanetUI()
    {

        stabilityT.text = ((int)planet.stability).ToString() + "%";
        crimeT.text = ((int)(planet.crime)).ToString() + "%";
        remainingJobSlotT.text = planet.remainingJobSlotNum.ToString();
        amenityT.text = planet.amenity.ToString();
        unemployedPopT.text = planet.unemployedPOPs.Count.ToString();
        averageHappinessT.text = ((int)planet.pops.Average(p => p.happiness)).ToString() + "%";
        housingT.text = planet.housing.ToString();
        popT.text = planet.pops.Count.ToString();
    }
}
