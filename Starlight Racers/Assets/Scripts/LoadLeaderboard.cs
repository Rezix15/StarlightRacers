using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;


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
    
    public enum TrackType
    {
        StarlightCity,
        Candyland
    }

    public TrackType currentMap;
    // Start is called before the first frame update
    void Start()
    {
        if (currentMap == TrackType.StarlightCity)
        {
            publicLeaderboardKey = "9cf23c0c92b506413137ee83c9f371e35e1f51a9c8e1ae1b937d167daf2232bd";
        }
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

    private void CalculateFinishTimeBonus()
    {
        int difficultyTime = 180 + (MenuManager.difficultyLevel * 60);
        finishTimeBonusScore = (int)((300 + (difficultyTime * 3)) - MenuManager.totalFinishTime);
        finishTimeBonusScore = (int)(finishTimeBonusScore * ((MenuManager.difficultyLevel * 0.1f)  + 0.9f)) * 10;
    }

    public void GetLeaderBoard()
    {
        var loopLength = 0;
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) =>
        {
            loopLength = msg.Length < hiScoreTexts.Length ? msg.Length : hiScoreTexts.Length;
            
            for (int i = 0; i < loopLength; i++)
            {
                hiScoreTexts[i].text =
                    " " + hiScoreTexts[i].gameObject.name + " " + msg[i].Username + " " + msg[i].Score + "PTS";
            }
        });
    }

    public void SetLeaderboardValue(string username)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((_) =>
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
