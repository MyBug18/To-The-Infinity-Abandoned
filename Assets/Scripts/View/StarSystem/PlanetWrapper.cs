using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWrapper : CelestialBodyWrapper
{
    public Planet planet;
    public Transform satellites;
    public Transform planetMesh;
    public Transform InhabitablePlanetUI;

    private Vector3 _initialScale;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        planet = (Planet)body;

        planetMesh.localScale *= planet.size / 15.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        planetMesh.GetComponent<MeshRenderer>().material.SetVector("_LightDirection", new Vector4(transform.localPosition.x, transform.localPosition.y, 0, 1));
    }
}
