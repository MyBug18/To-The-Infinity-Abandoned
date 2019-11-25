using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOnClick : MonoBehaviour
{
    [SerializeField]
    private PlanetWrapper _planetWrapper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log(_planetWrapper.planet.name);
    }
}
