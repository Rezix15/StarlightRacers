using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI laserAmmoText;

    private int laserAmmoAmount;
    private float playerShieldStat;
    private float playerShieldStatMax;

    [SerializeField] private float timer;

    private bool canStart;

    public GameObject shieldBarFill;
    public GameObject abilityBarFill;

    public Image shieldBarFillImg;

    public Slider shieldBar;
    public Slider abilityBar;

    public Spacejet spacejet;
    
    public TextMeshProUGUI raceCountText;

    public TextMeshProUGUI currentShieldText;
    public TextMeshProUGUI currentAbilityText;
    public TextMeshProUGUI shieldTitle;

    public GameObject[] boosterDisplays;

    private GameObject[] boosterStatusObj;

    private Image[] boostStatusIcon;

    private GameObject[] currentBoosterObj;
    private Image[] currentBoosterIcon;

    public Sprite buffIcon;
    public Sprite debuffIcon;
    
    public Sprite speedIcon;
    public Sprite shieldIcon;
    public Sprite shieldRateIcon;
    public Sprite laserDmgIcon;
    public Sprite defaultIcon;

    public static bool gamePaused = false;

    private Color safeColor;
    private Color warningColor;
    private Color dangerousColor;
    
    public GameObject mainUI;
    public GameObject gameOverUI;
    
    public GameObject successObj;

    //public static bool GameOver;
    
    // Start is called before the first frame update
    void Start()
    {
        //timerText.text = "00 : 00";
        spacejet = spacejet.GetComponent<Spacejet>();
        RaceManager.GameStarted += StartTimer1;
        StartCoroutine(StartTimer());

        safeColor = new Color(0, (191 / 255f), (173 / 255f), (128 / 255f));
        warningColor = new Color((255 / 255f), (191 / 255f), (13 / 255f), (128 / 255f));
        dangerousColor = new Color((191 / 255f), (43 / 255f), (0 / 255f), (128 / 255f));
        
        // timerText.text = "";
        laserAmmoText.text = "";
        
        playerShieldStatMax = spacejet.shieldMax.trueValue;
        shieldBar.maxValue = playerShieldStatMax;

        abilityBar.maxValue = GameDataManager.abilityGaugeMax;
        
        raceCountText.text = GameDataManager.RaceCount.ToString() + " / 3";

        boostStatusIcon = new Image[boosterDisplays.Length];
        currentBoosterIcon = new Image[boosterDisplays.Length];

        boosterStatusObj = GameObject.FindGameObjectsWithTag("BuffIcon");
        
        currentBoosterObj = GameObject.FindGameObjectsWithTag("currentBoostIcon");

        for (int i = 0; i < boosterDisplays.Length; i++)
        {
            boostStatusIcon[i] = boosterStatusObj[i].GetComponent<Image>();
            currentBoosterIcon[i] = currentBoosterObj[i].GetComponent<Image>();
        }
        
        UpdateBoosterUI();
    }

    private void Awake()
    {
        Debug.Log("TimerVal: " + GameDataManager.timerVal);
        timer = 180 + (MenuManager.difficultyLevel * 60) + GameDataManager.timerVal;
        FormatTimer(timer, timerText);
    }
    
    //Function to format the timer for prettiness
    private void FormatTimer(float timelimit, TextMeshProUGUI timelimitText)
    {
        int seconds = Mathf.FloorToInt(timelimit % 60); //calculates seconds.
        int minutes = Mathf.FloorToInt(timelimit / 60); //calculates minutes.
        int milliseconds = Mathf.FloorToInt(timelimit * 1000) % 1000; //calculates milliseconds.
        
        string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

        timelimitText.text = timedString;
    }
    
    void StartTimer1()
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

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        
        UpdateUI();
    }
    
    //Display Timer onto Canvas
    IEnumerator StartTimer()
    {
        while (spacejet != null && !spacejet.hasFinished )
        {
            FormatTimer(timer, timerText);
            yield return new WaitForSeconds(0);
        }
    }
    
    void UpdateUI()
    {
        if (spacejet != null)
        {
            laserAmmoAmount = (int)spacejet.GetLaserAmmoCount();

            laserAmmoText.text = laserAmmoAmount.ToString();

            playerShieldStat = spacejet.GetCurrentShieldStat();

            shieldBar.value = playerShieldStat;

            abilityBar.value = spacejet.ReturnPlayerGauge();

            var shieldAsInt = (int)playerShieldStat;

            currentShieldText.text = shieldAsInt.ToString();

            var abilityAsInt = (int)spacejet.ReturnPlayerGauge();

            currentAbilityText.text = abilityBar.value >= abilityBar.maxValue ? "READY" : abilityAsInt.ToString();
            
            UpdateShieldUI();
        }
        else
        {
            //GameDataManager.GameOver = true;
            mainUI.SetActive(false);
            gameOverUI.SetActive(true);
            StartCoroutine(SendUserBackToIntermission());
        }
        
        if (spacejet.hasFinished && Boss.bossReady)
        {
            successObj.SetActive(true);
        }
        
    }

    void UpdateShieldUI()
    {
        var shieldPercentage = (shieldBar.value / shieldBar.maxValue);

        if (shieldPercentage > 0)
        {
            shieldBarFill.SetActive(true);
        }

        switch (shieldPercentage)
        {
            case >= 0.5f:
            {
                shieldBarFillImg.color = Color.cyan;
                shieldTitle.fontMaterial.SetColor(Shader.PropertyToID("_GlowColor"), safeColor);
                currentShieldText.fontSharedMaterial.SetColor(Shader.PropertyToID("_GlowColor"), safeColor);
                break;
            }
                
            case < 0.5f and >= 0.2f:
            {
                shieldBarFillImg.color = Color.yellow;
                shieldTitle.fontMaterial.SetColor(Shader.PropertyToID("_GlowColor"), warningColor);
                currentShieldText.fontSharedMaterial.SetColor(Shader.PropertyToID("_GlowColor"), warningColor);
                break;
            }
                
            case < 0.2f and > 0:
            {
                shieldBarFillImg.color = Color.red;
                shieldTitle.fontMaterial.SetColor(Shader.PropertyToID("_GlowColor"), dangerousColor);
                currentShieldText.fontSharedMaterial.SetColor(Shader.PropertyToID("_GlowColor"), dangerousColor);
                break;
            }
            
            default:
            {
                shieldBarFill.SetActive(false);
                break;
            }
               
        }
    }

    void UpdateBoosterUI()
    {
        if (MenuManager.componentBoosts != null)
        {
            DisplayBoosterAmount(MenuManager.componentBoosts.Count); //Function to display the amount of boosters equipped
            
            for (int i = 0; i < MenuManager.componentBoosts.Count; i++)
            {
                CheckBoosterTarget(i); //Check the booster target stat to display the correct icon

                //If modifier is a buff, else it is a debuff
                if (MenuManager.componentBoosts[i].statModifierVal > 0)
                {
                    boostStatusIcon[i].sprite = buffIcon;
                }
                else
                {
                    boostStatusIcon[i].sprite = debuffIcon;
                }
                
                
            }
        }
        else
        {
            DisplayBoosterAmount(-1); //Ensures that the boosters are all invisible, when player has no boosts(buffs)
        }
        
    }

    //Function to check the target stat of each modifier equipped to booster.
    void CheckBoosterTarget(int index)
    {
        switch (MenuManager.componentBoosts[index].targetStat)
        {
            case ComponentObj.StatSkillType.Speed:
            {
                currentBoosterIcon[index].sprite = speedIcon;
                break;
            }

            case ComponentObj.StatSkillType.ShieldRate:
            {
                currentBoosterIcon[index].sprite = shieldRateIcon;
                break;
            }
            
            case ComponentObj.StatSkillType.Shield:
            {
                currentBoosterIcon[index].sprite = shieldIcon;
                break;
            }

            case ComponentObj.StatSkillType.Grip:
            {
                currentBoosterIcon[index].sprite = defaultIcon;
                break;
            }

            case ComponentObj.StatSkillType.Thrust:
            {
                currentBoosterIcon[index].sprite = defaultIcon;
                break;
            }

            case ComponentObj.StatSkillType.LaserDamage:
            {
                currentBoosterIcon[index].sprite = laserDmgIcon;
                break;
            }
            

            default:
            {
                break;
            }
        }
    }

    //Function to display the amount of boosters that the player has.
    void DisplayBoosterAmount(int count)
    {
        for (int i = 0; i < boosterDisplays.Length; i++)
        {
            boosterDisplays[i].SetActive(i < count);
        }
    }
    
    IEnumerator SendUserBackToIntermission()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("IntermissionScene");
    }
}
