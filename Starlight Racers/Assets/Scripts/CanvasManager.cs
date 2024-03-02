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

    public TextMeshProUGUI currentShieldText;

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
    
    // Start is called before the first frame update
    void Start()
    {
        //timerText.text = "00 : 00";
        spacejet = spacejet.GetComponent<Spacejet>();
        RaceManager.GameStarted += StartTimer;

        timerText.text = "";
        laserAmmoText.text = "";
        
        playerShieldStatMax = spacejet.shieldMax.trueValue;
        shieldBar.maxValue = playerShieldStatMax;
        
        raceCountText.text = MenuManager.RaceCount.ToString() + " / 3";

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
        if (!spacejet.hasFinished)
        {
            int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
            int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
            int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
            string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

            timerText.text = timedString;
        }
        
        laserAmmoAmount = spacejet.GetLaserAmmoCount();

        laserAmmoText.text = laserAmmoAmount.ToString();

        playerShieldStat = spacejet.GetCurrentShieldStat();

        shieldBar.value = playerShieldStat;

        var shieldAsInt = (int)playerShieldStat;

        currentShieldText.text = shieldAsInt.ToString();
        
        UpdateShieldUI();
        
        UpdateBoosterUI();
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

    void UpdateBoosterUI()
    {
        DisplayBoosterAmount(MenuManager.componentBoosts.Count); //Function to display the amount of boosters equipped
        
        if (MenuManager.componentBoosts.Count > 0)
        {
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
}
