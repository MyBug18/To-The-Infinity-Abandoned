﻿using System.Collections;
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

    private Coroutine _gameProceeder;
    private Game _game;

    private float currentInterval = 0;
    private int currentFrameCounter = 0;

    public List<StarSystemWrapper> systems = new List<StarSystemWrapper>();

    public delegate void ObjectInOrbitRotationEvent();

    public ObjectInOrbitRotationEvent objectInOrbitRotationEvents;



    public Planet_Inhabitable earth;


    private void Awake()
    {
        _game = GameDataHolder.game;
        _game.AddRandomStarSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateStarSystem(_game.systems[0]);
        Debug.Log(_game.systems[0]);



        earth = new Planet_Inhabitable("earth", 15, _game, null, 0, false);
        _game.colonizedPlanets.Add(earth);
        earth.BirthPOP();
        Debug.Log(earth);

        foreach (var p in earth.unemployedPOPs)
            Debug.Log(p);

        earth.StartConstruction(BuildingType.ColonizationCenter);
        earth.StartConstruction(DistrictType.Mineral);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(_game.globalResource);

        if (Input.GetKeyDown(KeyCode.W))
        {
            earth.pops[0].ActivatePOP(earth.buildings[0].workingPOPSlotList[0]);
            Debug.Log(earth.buildings[0]);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            earth.pops[0].MovePOPJob(earth.districts[0].workingPOPSlotList[0]);
            Debug.Log(earth.districts[0]);
        }


        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameDataHolder.isPaused)
            {
                PlayGame();
            }
            else
            {
                PauseGame();
            }
        }
        
        if (!GameDataHolder.isPaused)
        {
            currentInterval += Time.deltaTime;
            if (currentInterval > GameDataHolder.interval)
            {
                _game.IncreaseOneDay();
                date.text = _game.date;
                currentInterval = 0;
            }
            
            currentFrameCounter++;
            if (currentFrameCounter > GameDataHolder.gameSpeed)
            {
                _CalledByGameFrame();
                currentFrameCounter = 0;
            }
            
        }
    }
    
    private void _CalledByGameFrame()
    {
        objectInOrbitRotationEvents();
    }

    public void PlayGame()
    {
        if (!GameDataHolder.isPaused) return;
        GameDataHolder.isPaused = false;
    }

    public void PauseGame()
    {
        if (GameDataHolder.isPaused) return;
        GameDataHolder.isPaused = true;
    }    

    public void InstantiateStarSystem(StarSystem system) // Position should be modified.
    {
        Transform sys = Instantiate(starSystemPrefab, new Vector3(0, 0), Quaternion.identity, parentOfAllSystems);
        sys.name = system.name;
        sys.GetComponent<StarSystemWrapper>().system = system;
        sys.GetComponent<StarSystemWrapper>().gameManager = this;
    }
}
