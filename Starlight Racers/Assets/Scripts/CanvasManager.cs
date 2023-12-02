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

    public Slider shieldBar;

    public Spacejet spacejet;
    
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "00 : 00";
        spacejet = spacejet.GetComponent<Spacejet>();
        RaceManager.GameStarted += StartTimer;
        
        timerText.gameObject.SetActive(false);
        laserAmmoText.gameObject.SetActive(false);

        playerShieldStatMax = spacejet.GetMaxShieldStat();
        shieldBar.maxValue = playerShieldStatMax;

    }


    void StartTimer()
    {
        canStart = true;
        
        timerText.gameObject.SetActive(true);
        laserAmmoText.gameObject.SetActive(true);
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


    }
}
