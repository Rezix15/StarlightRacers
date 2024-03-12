using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCanvas : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI laserAmmoText;
    
    public GameObject shieldBarFill;
    public Image shieldBarFillImg;
    public Slider shieldBar;
    private float playerShieldStat;
    private float playerShieldStatMax;
    private float timer;
    public TextMeshProUGUI currentShieldText;
    public PlayerBoss player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = player.GetComponent<PlayerBoss>();
        timerText.text = "";
        laserAmmoText.text = "MAX";
        playerShieldStatMax = player.shieldMax.trueValue;
        shieldBar.maxValue = playerShieldStatMax;
        
        timer = 300; //Set boss to 5 minutes
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
        if (!player.hasFinished)
        {
            int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
            int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
            int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
            string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

            timerText.text = timedString;
        }
        
        playerShieldStat = player.GetCurrentShieldStat();

        shieldBar.value = playerShieldStat;

        var shieldAsInt = (int)playerShieldStat;

        currentShieldText.text = shieldAsInt.ToString();
        
        UpdateShieldUI();
        
        //UpdateBoosterUI();
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
        else if(shieldPercentage < 0.5f && shieldPercentage >= 0.2f)
        {
            shieldBarFillImg.color = Color.yellow;
        }
        else if (shieldPercentage < 0.2f && shieldPercentage > 0)
        {
            shieldBarFillImg.color = Color.red;
        }
        else
        {
            shieldBarFill.SetActive(false);
        }
    }
}
