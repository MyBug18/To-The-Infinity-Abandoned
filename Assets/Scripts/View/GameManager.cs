using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text date;

    public bool isPaused = false;
    public float interval = 0.15f;

    private Coroutine _gameProceeder;
    private Game _game;

    StarSystem sys;

    [SerializeField]
    Transform starSystemPrefab;

    private void Awake()
    {
        _game = GameDataHolder.game;
        _game.AddRandomStarSystem();
        sys = _game.systems[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameProceeder = StartCoroutine(_StartYear());
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
    
}
