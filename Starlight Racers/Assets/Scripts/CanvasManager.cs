using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI laserAmmoText;

    private int laserAmmoAmount;
    private float playerShieldStat;
    private float playerShieldStatMax;
    
    private float timer;

    private bool canStart;

    public GameObject shieldBarFill;

    public Image shieldBarFillImg;

    public Slider shieldBar;

    public Spacejet spacejet;
    
    public TextMeshProUGUI raceCountText;
    
    // Start is called before the first frame update
    void Start()
    {
        //timerText.text = "00 : 00";
        spacejet = spacejet.GetComponent<Spacejet>();
        RaceManager.GameStarted += StartTimer;

        timerText.text = "";
        laserAmmoText.text = "";

        playerShieldStatMax = MenuManager.currentSpaceJet.shield;
        shieldBar.maxValue = playerShieldStatMax;
        
        raceCountText.text = MenuManager.RaceCount.ToString() + " / 3";

    }


    void StartTimer()
    {
        canStart = true;
        
        //timerText.gameObject.SetActive(true);
        //laserAmmoText.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if(!canStart)
            return;
        
        timer += Time.deltaTime;
        
        UpdateUI();
    }
    
    void UpdateUI()
    {
        //Display Timer onto Canvas
        int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
        int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
        int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
        string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

        timerText.text = timedString;
        
        laserAmmoAmount = spacejet.GetLaserAmmoCount();

        laserAmmoText.text = laserAmmoAmount.ToString();

        playerShieldStat = spacejet.GetCurrentShieldStat();

        shieldBar.value = playerShieldStat;
        
        UpdateShieldUI();
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
