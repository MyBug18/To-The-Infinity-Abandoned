using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YearTest : MonoBehaviour
{
    [SerializeField]
    Text date;

    public bool isPaused = false;
    public float interval = 0.15f;
    Coroutine cor;
    Game game;
    
    private void Awake()
    {
        game = GameManager.game;
        var earth = new Planet_Inhabitable("Earth", 16, game);

        GameManager.game.planets.Add(earth);

        earth.BuildDistrict(DistrictType.Fuel);
        earth.BuildDistrict(DistrictType.Mineral);

        earth.BirthPOP();
        earth.BirthPOP();
    }

    // Start is called before the first frame update
    void Start()
    {
        cor = StartCoroutine(_StartYear());
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

        // 0: Fuel District (Upkeep: 0 - 0.5, 1 - 1)
        // 1: Mineral District (Upkeep: 0 - 0.5, 1 - 1)

        // 0: POP 0
        // 1: POP 1

        var earth = GameManager.game.planets[0];

        if (Input.GetKeyDown(KeyCode.G))
        {
            earth.pops[0].ActivatePOP((earth.districts[0], 0));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            earth.pops[1].ActivatePOP((earth.districts[1], 1));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            earth.districts[0].MovePOPJob(0, (earth.districts[1], 0));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            earth.districts[1].MovePOPJob(1, (earth.districts[0], 0));
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            earth.districts[1].MovePOPJob(0, (earth.districts[0], 0));
        }





        if (Input.GetKeyDown(KeyCode.F))
            foreach (var u in earth.planetJobUpkeeps)
                Debug.Log(u);        

        if (Input.GetKeyDown(KeyCode.D))
            for(int i = 0; i < 2; i++)
                Debug.Log(earth.districts[i]);

        if (Input.GetKeyDown(KeyCode.S))
            foreach (var p in earth.pops)
                Debug.Log(p);

        if (Input.GetKeyDown(KeyCode.A))
            foreach (var p in earth.unemployedPOPs)
                Debug.Log(p);



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
