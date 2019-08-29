using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text date;

    [SerializeField]
    Transform starSystemPrefab, parentOfAllSystems;

    public bool isPaused = false;
    public float interval = 0.15f;

    private Coroutine _gameProceeder;
    private Game _game;

    public List<StarSystemWrapper> systems = new List<StarSystemWrapper>();

    private void Awake()
    {
        _game = GameDataHolder.game;
        _game.AddRandomStarSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameProceeder = StartCoroutine(_StartYear());
        InstantiateStarSystem(_game.systems[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                PlayGame();
            }
            else
            {
                PauseGame();
            }
        }
        

    }

    public void PlayGame()
    {
        if (!isPaused) return;
        isPaused = false;
        _gameProceeder = StartCoroutine(_StartYear());
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        StopCoroutine(_gameProceeder);
    }

    IEnumerator _StartYear()
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            GameDataHolder.game.IncreaseOneDay();
            date.text = GameDataHolder.game.date;
        }
    }
    

    public void InstantiateStarSystem(StarSystem system) // Position should be modified.
    {
        Transform sys = Instantiate(starSystemPrefab, new Vector3(0, 0), Quaternion.identity, parentOfAllSystems);
        sys.name = system.name;
        sys.GetComponent<StarSystemWrapper>().system = system;
    }
}
