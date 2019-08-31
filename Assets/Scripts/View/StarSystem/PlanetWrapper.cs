using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWrapper : CelestialBodyWrapper
{
    public Planet planet;
    public Transform satellites;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        planet = (Planet)body;
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
