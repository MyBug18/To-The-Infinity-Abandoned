using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CelestialBodyUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private InhabitablePlanetUI ipui;

    [SerializeField]
    private CelestialBodyUI cbui;

    private bool isUIActivated = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isUIActivated)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                isUIActivated = true;
                if (hit.transform.parent.gameObject.GetComponent<CelestialBodyWrapper>() is CelestialBodyWrapper cbw)
                {
                    Debug.Log(cbw.body.name);

                    if (cbw.body is Planet_Inhabitable planet && planet.isColonized) // For colonized planets
                    {
                        ipui.planet = planet;
                        ipui.gameObject.SetActive(true);
                    }
                    else
                    {
                        cbui.body = cbw.body;
                        cbui.gameObject.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ipui.gameObject.SetActive(false);
            cbui.gameObject.SetActive(false);
            isUIActivated = false;
        }
    }
}
