using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CelestialBodyUI : MonoBehaviour
{
    public CelestialBody body { private get; set; }

    [SerializeField]
    private Text cbNameT, bodyTypeT, cbResourcesT, cbDescriptionT;

    private void OnEnable()
    {
        _Initiallize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _Initiallize()
    {
        cbNameT.text = body.name;
        _InitiallizeBodyType();
    }

    private void _InitiallizeBodyType()
    {
        if (body is Planet p)
        {
            switch(p.planetType)
            {
                case PlanetType.Inhabitable:
                    bodyTypeT.text = "Inhabitable Planet";
                    break;
                case PlanetType.Barren:
                    bodyTypeT.text = "Barren Planet";
                    break;
                case PlanetType.GasGiant:
                    bodyTypeT.text = "Gas Giant";
                    break;
                case PlanetType.Frozen:
                    bodyTypeT.text = "Frozen Planet";
                    break;
                case PlanetType.Molten:
                    bodyTypeT.text = "Molten Planet";
                    break;
                case PlanetType.Broken:
                    bodyTypeT.text = "Broken Planet";
                    break;
                case PlanetType.Crystal:
                    bodyTypeT.text = "Crystal Planet";
                    break;
                case PlanetType.Robotic:
                    bodyTypeT.text = "Robotic Planet";
                    break;
                case PlanetType.Shrouded:
                    bodyTypeT.text = "Shrouded Planet";
                    break;
                case PlanetType.Hive:
                    bodyTypeT.text = "Hive Planet";
                    break;
                default:
                    throw new NotImplementedException("Planet Type Not Implemented!");
            }
        }
        else
        {
            throw new NotImplementedException("Implement other body types!");
        }
    }
}
