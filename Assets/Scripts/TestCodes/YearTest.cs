using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class YearTest : MonoBehaviour
{
    [SerializeField]
    Text date;

    public bool isPaused = false;
    public float interval = 0.15f;
    Coroutine cor;
    Game game;
    Planet_Inhabitable earth;

    StarSystem sys;
    
    private void Awake()
    {
        game = GameManager.game;
        earth = new Planet_Inhabitable("Earth", 16, game, null, 0);
        earth.StartColonization();
        sys = new StarSystem(game);
    }

    // Start is called before the first frame update
    void Start()
    {
        cor = StartCoroutine(_StartYear());
        Debug.Log(sys);
        Debug.Log(sys.orbits.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                isPaused = false;
                cor = StartCoroutine(_StartYear());
            }
            else
            {
                isPaused = true;
                StopCoroutine(cor);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
            earth.KillPOP(earth.pops[0]);

        if (Input.GetKeyDown(KeyCode.G))
        {
            earth.BirthPOP();
        }       

        if (Input.GetKeyDown(KeyCode.T))
        {
            earth.StartConstruction(DistrictType.Fuel);
        }

        if (Input.GetKeyDown(KeyCode.H))
            earth.StartUpgrade(earth.buildings[0]);




        if (Input.GetKeyDown(KeyCode.F))
            foreach (var u in earth.planetJobUpkeeps)
                Debug.Log(u);

        if (Input.GetKeyDown(KeyCode.D))
            foreach (var d in earth.districts)
                Debug.Log(d.name);

        if (Input.GetKeyDown(KeyCode.S))
            foreach (var p in earth.pops)
                Debug.Log(p);

        if (Input.GetKeyDown(KeyCode.A))
            foreach (var p in earth.unemployedPOPs)
                Debug.Log(p);

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var y in earth.planetJobYields)
                Debug.Log(y);
        }

        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log(game.globalResource);

        if (Input.GetKeyDown(KeyCode.W))
            foreach (var b in earth.buildings)
                Debug.Log(b.name);

        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(earth);

        if (Input.GetKeyDown(KeyCode.M))
        {
            for (int i = 0; i < 50; i++)
                earth.BirthPOP();
        }


    }

    IEnumerator _StartYear()
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            GameManager.game.IncreaseOneDay();
            date.text = GameManager.game.date;
        }
    }
    
}
