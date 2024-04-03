using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadLeaderboard : MonoBehaviour
{
    private PlayerBoss player;
    public TextMeshProUGUI countdownText;

    public GameObject leaderboardUI;
    public GameObject usernameInput;
    public TextMeshProUGUI scoreText;

    [SerializeField]
    private bool hasFinished;

    private int score;

    private int finishTimeBonusScore;

    private int BossBonusScore;

    private int playerHealthBonus;

    public TextMeshProUGUI scoreInfoText;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    private void CalculateHealthBonusScore()
    {
        float currentHealth = player.GetCurrentShieldStat();
        float maxHealth = player.shieldMax.trueValue;
        float shieldRatio = (currentHealth / maxHealth);

        if (shieldRatio >= 0.99f)
        {
            playerHealthBonus = 500;
        }
        else if(shieldRatio >= 0.7f && shieldRatio < 0.99f)
        {
            playerHealthBonus = 400;
        }
        else if(shieldRatio >= 0.5f && shieldRatio < 0.7f)
        {
            playerHealthBonus = 300;
        }
        else if(shieldRatio >= 0.2f && shieldRatio < 0.5f)
        {
            playerHealthBonus = 200;
        }
        else 
        {
            playerHealthBonus = 100;
        }
    }

    private void CalculateFinishTimeBonus()
    {
        int difficultyTime = 180 + (MenuManager.difficultyLevel * 60);
        finishTimeBonusScore = (int)((300 + (difficultyTime * 3)) - MenuManager.totalFinishTime);
        finishTimeBonusScore = (int)(finishTimeBonusScore * ((MenuManager.difficultyLevel * 0.1f)  + 0.9f)) * 10;
    }
}
