using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InhabitablePlanetUIManager : MonoBehaviour
{
    public Planet_Inhabitable planet { private get; set; }

    [SerializeField]
    private Text planetNameT, planetSizeT, stabilityT, crimeT, remainingJobSlotT, amenityT, housingT, unemployedPopT, averageHappinessT; 

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

        _UpdateUI();

    }

    private void _UpdateUI()
    {

        stabilityT.text = planet.stability.ToString("0.0");
        crimeT.text = planet.crime.ToString("0.0");
        remainingJobSlotT.text = planet.remainingJobSlotNum.ToString();
        amenityT.text = planet.amenity.ToString("0.0");
        unemployedPopT.text = planet.unemployedPOPs.Count.ToString();
        averageHappinessT.text = planet.pops.Average(p => p.happiness).ToString("0.0");
    }
}
