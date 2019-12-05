using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIManager : MonoBehaviour
{
    [SerializeField]
    private PlanetWrapper _planetWrapper;

    [SerializeField]
    private Transform canvas;

    public Transform mainUI;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log(_planetWrapper.planet.name);
        InstantiateUI(_planetWrapper.planet);
    }

    public void InstantiateUI(Planet body)
    {
        Transform _mainUI = Instantiate(mainUI,canvas);
        mainUI.GetChild(0).GetComponent<Text>().text = body.name;
    }

}
