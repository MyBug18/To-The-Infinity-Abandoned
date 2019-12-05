using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform canvas, mainUI;


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
                if (hit.transform.parent.gameObject.GetComponent<CelestialBodyWrapper>() is CelestialBodyWrapper w)
                {
                    Debug.Log(w.body.name);
                    var _mainUI = Instantiate(mainUI, canvas);
                    _mainUI.GetChild(0).GetComponent<Text>().text = w.body.name;
                }
            }
        }
    }
}
