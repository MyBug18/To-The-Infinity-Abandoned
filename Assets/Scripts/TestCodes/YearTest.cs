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
    Planet_Inhabitable earth;
    
    private void Awake()
    {
        game = GameManager.game;
        earth = new Planet_Inhabitable("Earth", 16, game);
        earth.StartColonization();
        /*
        earth.BuildDistrict(DistrictType.Fuel);
        earth.BuildDistrict(DistrictType.Mineral);

        earth.BirthPOP();
        earth.BirthPOP();
        */
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

        if (Input.GetKeyDown(KeyCode.V))
            earth.KillPOP(earth.pops[0]);

        if (Input.GetKeyDown(KeyCode.G))
        {
            earth.BirthPOP();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            earth.BuildDistrict(DistrictType.Fuel);
            earth.BuildDistrict(DistrictType.Mineral);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            earth.pops[0].ActivatePOP(earth.districts[0], 0);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var y in earth.planetJobYields)
                Debug.Log(y);
        }

        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log(game.globalResource);

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.SaveGame();
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
