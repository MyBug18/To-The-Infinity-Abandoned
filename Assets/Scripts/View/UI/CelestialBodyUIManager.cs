using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CelestialBodyUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private InhabitablePlanetUIManager ipm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.parent.gameObject.GetComponent<CelestialBodyWrapper>() is CelestialBodyWrapper cbw)
                {
                    Debug.Log(cbw.body.name);

                    if (cbw.body is Planet_Inhabitable planet && planet.isColonized)
                    {
                        ipm.planet = planet;
                        ipm.gameObject.SetActive(true);
                    }
                    else
                    {
                    }
                }
            }
        }
    }
}
