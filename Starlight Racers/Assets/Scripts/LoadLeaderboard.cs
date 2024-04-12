using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;
using UnityEngine.EventSystems;


//Leaderboard server code heavily inspired by https://www.youtube.com/watch?v=-O7zeq7xMLw
public class LoadLeaderboard : MonoBehaviour
{
    private PlayerBoss player;
    public TextMeshProUGUI countdownText;

    public GameObject leaderboardUI;
    public GameObject currentLeaderboard;
    
    public GameObject usernameInput;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI[] hiScoreTexts;

    [SerializeField]
    private bool hasFinished;

    private int score;

    private int finishTimeBonusScore;

    private int BossBonusScore;

    private int playerHealthBonus;

    public TextMeshProUGUI scoreInfoText;

    public TMP_InputField userNameField;

    private string playerName;

    private string publicLeaderboardKey;

    private string difficulty;
    
    public enum TrackType
    {
        StarlightCity,
        Candyland
    }

    public TrackType currentMap;
    // Start is called before the first frame update
    void Start()
    {
        switch (currentMap)
        {
            case TrackType.StarlightCity:
            {
                publicLeaderboardKey = "6f39a5bf4675da96e470c2be6f2300c394b876400794b2ef81887d2ab992205a";
                break;
            }

            case TrackType.Candyland:
            {
                publicLeaderboardKey = "673ec08c60a0c798908cbaa379bdcf75691ccb21854226ae93b80e0e040e5cdc";
                break;
            }
            
        }

        switch (MenuManager.difficultyLevel)
        {
            case 0:
            {
                difficulty = "Easy";
                break;
            }

            case 1:
            {
                difficulty = "Medium";
                break;
            }

            case 2:
            {
                difficulty = "Hard";
                break;
            }

            default:
            {
                difficulty = "Easy";
                break;
            }
        }
        
        GetLeaderBoard();
        leaderboardUI.SetActive(false);
        usernameInput.SetActive(false);
        player = GameObject.FindGameObjectWithTag("PlayerRacer").GetComponent<PlayerBoss>();
        countdownText.text = "";
        scoreInfoText.text = "";
        MenuManager.totalFinishTime += player.ReturnFinishTime();
        //hasFinished = player.hasFinished;
    }

    private void OnEnable()
    {
        BossBonusScore = 40000;
        StartCoroutine(WinningMessage());
    }

    // Update is called once per frame
    void Update()
    {
        hasFinished = player.hasFinished;
        scoreText.text = "Your Score: " + score;
    }
    
    IEnumerator WinningMessage()
    {
        print("Finishing..");
        yield return new WaitForSeconds(2f);
        countdownText.text = "YOU WIN!";
        yield return new WaitForSeconds(5f);
        countdownText.text = "";
        yield return new WaitForSeconds(2f);
        leaderboardUI.SetActive(true);
        CalculateHealthBonusScore();
        CalculateFinishTimeBonus();
        
        
        while (score < BossBonusScore)
        {
            score+= 1000;
            scoreInfoText.text = "Boss Completion Bonus: +" + BossBonusScore; 
            yield return new WaitForSeconds(0.05f);
        }

        
        scoreInfoText.text = ""; 
        yield return new WaitForSeconds(0.4f);
        
        
        while (score < BossBonusScore + playerHealthBonus)
        {
            
            scoreInfoText.text = "Health Bonus: +" + playerHealthBonus; 
            score+= 10;
            yield return new WaitForSeconds(0.05f);
        }
        
        scoreInfoText.text = ""; 
        yield return new WaitForSeconds(0.4f);
        
        while (score < BossBonusScore + playerHealthBonus + finishTimeBonusScore)
        {
            scoreInfoText.text = "TotalFinishTime Bonus: +" + finishTimeBonusScore; 
            score+=100;
            
            yield return new WaitForSeconds(0.05f);
        }

        //score = score + finishTimeBonusScore;
        
        scoreInfoText.text = "";
        yield return new WaitForSeconds(2f);
        usernameInput.SetActive(true);
        EventSystem.current.SetSelectedGameObject(usernameInput);
        yield return new WaitForSeconds(0.1f);
    }

    private void CalculateHealthBonusScore()
    {
        float currentHealth = player.GetCurrentShieldStat();
        float maxHealth = player.shieldMax.trueValue;
        float shieldRatio = (currentHealth / maxHealth);

        switch (shieldRatio)
        {
            case >= 0.99f:
            {
                playerHealthBonus = 500;
                break;
            }

            case >= 0.7f and < 0.99f:
            {
                playerHealthBonus = 400;
                break;
            }
                
            case >= 0.5f and < 0.7f:
            {
                playerHealthBonus = 300;
                break;
            }
            
            case >= 0.2f and < 0.5f:
            {
                playerHealthBonus = 200;
                break;
            }
            
            default:
            {
                playerHealthBonus = 100;
                break;
            }
            
        }
    }

    //Function to ca
    private void CalculateFinishTimeBonus()
    {
        int difficultyTime = 180 + (MenuManager.difficultyLevel * 60);
        finishTimeBonusScore = (int)((300 + (difficultyTime * 3)) - MenuManager.totalFinishTime);
        finishTimeBonusScore = (int)(finishTimeBonusScore * ((MenuManager.difficultyLevel * 0.1f)  + 0.9f)) * 100;
    }

    private void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) =>
        {
            var loopLength = 0;
            int counter = 0;
            
            if (msg.Length < hiScoreTexts.Length)
            {
                loopLength = msg.Length;
            }
            else
            {
                loopLength = hiScoreTexts.Length;
            }
            
            
            
            for (int i = 0; i < loopLength; i++)
            {
                if (msg[i].Extra == difficulty)
                {
                    hiScoreTexts[counter].text =
                        " " + hiScoreTexts[counter].gameObject.name + " " + msg[i].Username + " " + msg[i].Score + " PTS";

                    counter++;
                }
            }
        });
    }

    private void SetLeaderboardValue(string username)
    {
        LeaderboardCreator.ResetPlayer();
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, difficulty,  ((msg) =>
        {
            GetLeaderBoard();
        }));
        
    }

    public void GetUsername()
    {
        playerName = userNameField.text;
        SetLeaderboardValue(playerName);
        leaderboardUI.SetActive(false);
        currentLeaderboard.SetActive(true);
    }
}
