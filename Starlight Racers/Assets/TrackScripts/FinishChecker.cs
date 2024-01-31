using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishChecker : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI finishTimer;
    private float playerFinishTime;
    private string playerFinishTimeText;

    private Spacejet player;

    private RaceManager RaceManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //player = player.GetComponent<Spacejet>();
        var countdownObj = GameObject.FindGameObjectWithTag("Countdown");
        countdownText = countdownObj.GetComponent<TextMeshProUGUI>();

        var finishTimerObj = GameObject.FindGameObjectWithTag("FinishTimer");
        finishTimer = finishTimerObj.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private string FormatTimer(float timer)
    {
        //Display Timer onto Canvas
        int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
        int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
        int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
        string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

        return timedString;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RacerEnemy"))
        {
            Debug.Log(other.name + " has reached the goal");
        }
        else if(other.CompareTag("Player"))
        {
            StartCoroutine(PlayerFinished());
            player = other.gameObject.GetComponentInParent<Spacejet>();
            Debug.Log("Player: " + player);
            playerFinishTime = player.ReturnFinishTime();
            playerFinishTimeText = FormatTimer(playerFinishTime);

            finishTimer.text = playerFinishTimeText;
            finishTimer.gameObject.SetActive(true);
            
            if (MenuManager.RaceCount < 3)
            {
                MenuManager.RaceCount++;
                SceneManager.LoadScene("StarLightRacers_BetaTest");
            }
            
            
            
        }
    }

    IEnumerator PlayerFinished()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "FINISH!";
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
    }
    
    
}
