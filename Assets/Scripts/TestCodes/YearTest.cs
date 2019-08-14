using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearTest : MonoBehaviour
{
    [SerializeField]
    Text text;

    public bool isPaused = false;

    private void Awake()
    {
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_StartYear());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator _StartYear()
    {
        while (isPaused)
            yield return null;


        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            Game.IncreaseOneDay();
            text.text = Game.date;
        }

    }



}
