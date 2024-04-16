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
    public TextMeshProUGUI countdownTextP2;
    public TextMeshProUGUI finishTimer;
    public TextMeshProUGUI finishTimerP2;
    private float playerFinishTime;
    private string playerFinishTimeText;

    private Spacejet player;

    private RaceManager RaceManager;

    private bool hasFinished;

    private int finishedPlayers;

    private int playerCount;
    
    // Start is called before the first frame update
    void Start()
    {
        //player = player.GetComponent<Spacejet>();
        var countdownObj = GameObject.FindGameObjectsWithTag("Countdown");
        var finishTimerObj = GameObject.FindGameObjectsWithTag("FinishTimer");

        
        countdownText = countdownObj[0].GetComponent<TextMeshProUGUI>();
        finishTimer = finishTimerObj[0].GetComponent<TextMeshProUGUI>();
        
        
        hasFinished = false;

        //playerCount = GameObject.FindGameObjectsWithTag("PlayerRacer").Length;

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
        else if(other.CompareTag("Player") && hasFinished == false)
        {
            player = other.gameObject.GetComponentInParent<Spacejet>();
            StartCoroutine(PlayerFinished(player.isPlayer2));
            Debug.Log("Player: " + player);
            playerFinishTime = player.ReturnFinishTime();
            playerFinishTimeText = FormatTimer(playerFinishTime);

            finishTimer.text = playerFinishTimeText;
            finishTimer.gameObject.SetActive(true);

            if (!player.hasFinished)
            {
                finishedPlayers++;
            }
            
            // if (GameDataManager.RaceCount < 3 )
            // {
                GameDataManager.RaceCount++;
                MenuManager.totalFinishTime += playerFinishTime;
                CoinManager.coinCount += 1000;

                if (playerCount < 2 || (playerCount >= 2 && finishedPlayers >=2))
                {
                    switch (MenuManager.currentStageId)
                    {
                        case 0:
                        {
                            SceneManager.LoadScene("IntermissionScene");   
                            break;
                        }

                        case 1:
                        {
                            SceneManager.LoadScene("IntermissionScene(CandyLand)");   
                            break;
                        }
                    }
                    
                }
                
            //}
            // else
            // {
            //     playerFinishTimeText = FormatTimer(MenuManager.totalFinishTime);
            //     finishTimer.text = "Finish Time: " + playerFinishTimeText;
            // }
            
            //StartCoroutine(FinishTimeCountdown(player.isPlayer2));
        }
    }

    IEnumerator PlayerFinished(bool isPlayer2)
    {
        if (isPlayer2)
        {
            countdownTextP2.gameObject.SetActive(true);
            countdownTextP2.text = "FINISH!";
            yield return new WaitForSeconds(1);
            countdownTextP2.gameObject.SetActive(false);
        }
        else
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "FINISH!";
            yield return new WaitForSeconds(1);
            countdownText.gameObject.SetActive(false);
        }
        
    }

    IEnumerator FinishTimeCountdown(bool isPlayer2)
    {
        if (isPlayer2)
        {
            yield return new WaitForSeconds(4);
            finishTimerP2.text = "";
        }
        else
        {
            yield return new WaitForSeconds(4);
            finishTimer.text = "";
        }
        
        if(playerCount < 2 || (playerCount >= 2 && finishedPlayers >=2))
        {
            hasFinished = true;
        }
    }
    
    
}
