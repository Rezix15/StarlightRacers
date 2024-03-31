using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossCanvas : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI laserAmmoText;
    
    public GameObject shieldBarFill;
    public Image shieldBarFillImg;
    public Slider shieldBar;
    public Slider abilityGaugeBar;
    public TextMeshProUGUI currentAbilityGaugeText;
    private float playerShieldStat;
    private float playerShieldStatMax;
    private float timer;
    public TextMeshProUGUI currentShieldText;
    public PlayerBoss player;

    public GameObject mainUI;
    public GameObject gameOverUI;

    public static bool GameOver;

    public GameObject successObj;
    
    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;
        player = player.GetComponent<PlayerBoss>();
        timerText.text = "";
        laserAmmoText.text = "MAX";
        playerShieldStatMax = player.shieldMax.trueValue;
        shieldBar.maxValue = playerShieldStatMax;
        abilityGaugeBar.maxValue = player.abilityActivator;
        mainUI.SetActive(true);
        gameOverUI.SetActive(false);
        timer = 300; //Set boss to 5 minutes
        StartCoroutine(StartTimer());
        successObj.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        //Display Timer onto Canvas
        if (player != null)
        {
            playerShieldStat = player.GetCurrentShieldStat();

            shieldBar.value = playerShieldStat;
            abilityGaugeBar.value = player.ReturnPlayerGauge();
            

            var shieldAsInt = (int)playerShieldStat;
            var abilityAsInt = (int)player.ReturnPlayerGauge();

            currentShieldText.text = shieldAsInt.ToString();

            if (abilityGaugeBar.value >= abilityGaugeBar.maxValue)
            {
                currentAbilityGaugeText.text = "READY!";
            }
            else
            {
                currentAbilityGaugeText.text = abilityAsInt.ToString();
            }
            
        
            UpdateShieldUI();
        }
        else
        {
            GameOver = true;
            mainUI.SetActive(false);
            gameOverUI.SetActive(true);
            StartCoroutine(SendUserBackToIntermission());
        }

        if (player.hasFinished)
        {
            successObj.SetActive(true);
        }
       
        
        //UpdateBoosterUI();
    }

    IEnumerator StartTimer()
    {
        while (player != null && !player.hasFinished)
        {
            int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
            int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
            int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
            string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

            timerText.text = timedString;

            yield return new WaitForSeconds(0f);
        }
    }

    void UpdateShieldUI()
    {
        var shieldPercentage = (shieldBar.value / shieldBar.maxValue);

        if (shieldPercentage > 0)
        {
            shieldBarFill.SetActive(true);
        }

        if (shieldPercentage >= 0.5f)
        {
            shieldBarFillImg.color = Color.cyan;
        }
        else if (shieldPercentage < 0.5f && shieldPercentage >= 0.2f)
        {
            shieldBarFillImg.color = Color.yellow;
        }
        else if (shieldPercentage < 0.2f && shieldPercentage > 0)
        {
            shieldBarFillImg.color = Color.red;
        }
        else
        {
            currentShieldText.text = 0.ToString();
        }
    }

    IEnumerator SendUserBackToIntermission()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("IntermissionScene");
    }
    
}
