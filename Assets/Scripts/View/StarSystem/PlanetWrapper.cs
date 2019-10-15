using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWrapper : CelestialBodyWrapper
{
    public Planet planet;
    public Transform satellites;
    public Transform planetSprite;

    private Vector3 _initialScale;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        planet = (Planet)body;

        planetSprite.localScale *= planet.size / 15.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
