using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{
    public Material spaceJetColor;

    private Color[] materialColors;

    public List<Transform> menus; //List to store all the menus that are in our scene

    private int menu;

    private bool isActionPressed;

    private PlayerController Controller;

    private bool hasBeenPressed = false;

    public TextMeshProUGUI descriptiveText1;
    public TextMeshProUGUI difDescriptiveText; //Descriptive text for the difficulty menu

    public static SpaceJetObj currentSpaceJet;
    public static SpaceJetObj enemySpaceJet;

    public SpaceJetObj[] spaceJets; //Array that stores all the spacejets 

    private List<SpaceJetObj> availableSpaceJets; //Array that stores all the remaining spacejets when allocating a random one

    public TextMeshProUGUI spaceJetNameText;

    
    
    public static List<ComponentObj> componentBoosts;

    [SerializeField]
    private int difficultyMenuIndex;

    public GameObject diffButton;

    public GameObject diffOptionIndicator;

    //public static int score;
    
    # region SpaceJetMenu
    
    public GameObject speedStat;
    public GameObject shieldStat;
    public GameObject shieldRateStat;
    public GameObject gripStat;
    public GameObject thrustStat;
    public GameObject laserStat;

    private Image[] speedStatImg;
    private Image[] shieldStatImg;
    private Image[] shieldRateStatImg;
    private Image[] gripStatImg;
    private Image[] thrustStatImg;
    private Image[] laserStatImg;
    
    //Indicator to show what slide is current on
    private int currentId = 0;

    public GameObject lButton;
    public GameObject rButton;

    public GameObject prefabSpaceJetHolder;

    private GameObject[] prefabSpaceJets;
    
    # endregion
    
    
    # region
    public GameObject level1;
    public GameObject level2;
    public GameObject level1Indicator;
    public GameObject level2Indicator;
    # endregion
    

    private RaceManager RaceManager;

    public Image[] indicators;

    //public Image[] difficultyMenuIndicators;

    public Button[] menuOptions;

    public Button[] difMenuOptions;

    private int menuIndex;

    public static float totalFinishTime;

    public GameObject startButton;

    private TextMeshProUGUI startButtonText; //Text that is used to ask for user Input

    private bool isKeyboard; //Bool to check whether input is keyboard
    private bool isGamepad; //Bool to check whether input is gamepad

    private PlayerInput playerInput;

    private GameObject lastHoveredObj; //The last selected button, the player has chosen

    public Image submitButtonIcon;

    public TextMeshProUGUI specialAbilityText;
    public TextMeshProUGUI specialAbilityDesc;

    public static int difficultyLevel; //global variable that will be used to define the difficulty level for our game.
    public static int scaleLevel; //global variable that will set the scale of the track.
    public static int reachLimit; //global variable that sets the maximum limit co-ordinate the track gen can go

    private bool wasKeyboard;

    public TextMeshProUGUI bossNameText;

    public GameObject selectVehicleIndicator;

    public TextMeshProUGUI difficultyTimerText;

    private float timelimitVal;

    private string[] difDescText = new string[3];
      
 

    // public enum Level
    // {
    //     Starlight = 0,
    //     CandyLand = 1
    // }

    public static int currentStageId;
    
    private void Awake()
    {
        Controller = new PlayerController();

        Controller.MainMenu.Submit.performed += _ => isActionPressed = true;
        Controller.MainMenu.Submit.canceled += _ => isActionPressed = false;
    }

    private void OnEnable()
    {
        Controller.Enable();
    }

    private void OnDisable()
    {
        Controller.Enable();
    }
    // Start is called before the first frame update



    // Start is called before the first frame update
    void Start()
    {
        CoinManager.coinCount = 0;
        playerInput = GetComponent<PlayerInput>();
        startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        ToggleMenu(0);
        ToggleStartMenu(0);
        scaleLevel = 45;
        wasKeyboard = false;
        
        

        // speedStatImg = new Image[10];
        // shieldStatImg = new Image[10];
        // shieldRateStatImg = new Image[10];
        // gripStatImg = new Image[10];
        // thrustStatImg = new Image[10];
        // laserStatImg = new Image[10];

        speedStatImg = speedStat.GetComponentsInChildren<Image>();
        shieldStatImg = shieldStat.GetComponentsInChildren<Image>();
        shieldRateStatImg = shieldRateStat.GetComponentsInChildren<Image>();
        gripStatImg = gripStat.GetComponentsInChildren<Image>();
        thrustStatImg = thrustStat.GetComponentsInChildren<Image>();
        laserStatImg = laserStat.GetComponentsInChildren<Image>();

        currentSpaceJet = spaceJets[0];

        var childCount = prefabSpaceJetHolder.transform.childCount;

        prefabSpaceJets = new GameObject[childCount];

        difficultyLevel = 1; //Set the difficulty level to be at 1, which is normal mode
        

        
        //The lists that stores the amount of component boosts that the player has
        componentBoosts = new List<ComponentObj>();
        
        //Descriptive texts for the difficulty menu
        difDescText[0] = "Great for new beginners, who are unfamiliar with the mechanics. ";

        difDescText[1] = "Great for players who are not looking for an easy challenge but also not a difficult one.";
            
        difDescText[2] = "For players who seek a great challenge and experience the true thrill of racing. ";
        

        for (int i = 0; i < childCount; i++)
        {
            prefabSpaceJets[i] = prefabSpaceJetHolder.transform.GetChild(i).gameObject;

            if (i != currentId)
            {
                prefabSpaceJets[i].SetActive(false);
            }
            else
            {
                prefabSpaceJets[i].SetActive(true);
            }
        }

        availableSpaceJets = new List<SpaceJetObj>(spaceJets);
        difDescriptiveText.text = difDescText[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("wasKeyboard: " + wasKeyboard);
        UpdateUserInputDisplay();
        ToggleThroughMenu();
        //ToggleThroughDifficultyMenu();
        
    }
    
    public void StartGame()
    {
        //ToggleMenu(1);
        
        ToggleStartMenu(1);
        
        // if (playerInput.currentControlScheme == "Controller")
        // {
        //     EventSystem.current.SetSelectedGameObject(menuOptions[0].gameObject);
        //     lastHoveredObj = menuOptions[0].gameObject;
        // }
        
        EventSystem.current.SetSelectedGameObject(menuOptions[0].gameObject);
        lastHoveredObj = menuOptions[0].gameObject;
    }

    public void ExitMenu()
    {
        ToggleStartMenu(0);
        
        if (playerInput.currentControlScheme == "Controller")
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }

    private void ToggleThroughMenu()
    {
        GameObject selectedOpt = EventSystem.current.currentSelectedGameObject;
        lastHoveredObj = selectedOpt;
        
        //Debug.Log("lastHoveredObj: " + lastHoveredObj.name);
        
        // if (isGamepad)
        // {
        //     for (int i = 0; i < menuOptions.Length; i++)
        //     {
        //         indicators[i].gameObject.SetActive(menuOptions[i].gameObject == selectedOpt);
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i < menuOptions.Length; i++)
        //     {
        //         indicators[i].gameObject.SetActive(false);
        //     }
        // }
        
        for (int i = 0; i < menuOptions.Length; i++)
        {
            indicators[i].gameObject.SetActive(menuOptions[i].gameObject == selectedOpt);
        }
        
    }

    public void ToggleThroughDifficultyMenu(int type)
    {
        
        //GameObject selectedOpt = EventSystem.current.currentSelectedGameObject;
        //
        // // if (isGamepad)
        // // {
        // //     for (int i = 0; i < difMenuOptions.Length; i++)
        // //     {
        // //         difficultyMenuIndicators[i].gameObject.SetActive(difMenuOptions[i].gameObject == selectedOpt);
        // //     }
        // // }
        // // else
        // // {
        // //     for (int i = 0; i < difMenuOptions.Length; i++)
        // //     {
        // //         difficultyMenuIndicators[i].gameObject.SetActive(false);
        // //     }
        // // }
        //
        // for (int i = 0; i < difMenuOptions.Length; i++)
        // {
        //     difficultyMenuIndicators[i].gameObject.SetActive(difMenuOptions[i].gameObject == selectedOpt);
        // }
        
        //Add or subtract depending on the position of the button (-1 for left and + 1 for right)
        difficultyMenuIndex+= type;
        
        difficultyMenuIndex = difficultyMenuIndex % difMenuOptions.Length;
        
        if (difficultyMenuIndex == -1)
        {
            difficultyMenuIndex = 2;
        }

        //Set the time limit based on the difficulty. Starting from 180 seconds or 3 minutes up to 5 minutes (300 seconds)
        timelimitVal = (difficultyMenuIndex * 60) + 300;

        for (int i = 0; i < difMenuOptions.Length; i++)
        {
            difMenuOptions[i].gameObject.SetActive(i == difficultyMenuIndex);
        }
        
        //Set the descriptive text for the difficulty menu
        difDescriptiveText.text = difDescText[difficultyMenuIndex];
        
        //Format the timer for pretty printing
        FormatTimer(timelimitVal, difficultyTimerText);
        
        
    }

    //Function to format the timer for prettiness
    private void FormatTimer(float timer, TextMeshProUGUI timerText)
    {
        int seconds = Mathf.FloorToInt(timer % 60); //calculates seconds.
        int minutes = Mathf.FloorToInt(timer / 60); //calculates minutes.
        int milliseconds = Mathf.FloorToInt(timer * 1000) % 1000; //calculates milliseconds.
        
        string timedString = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

        timerText.text = timedString;
    }
    
    

    //Function that causes the descriptive text to change depending on the id of the button
    public void HoverButton(int id)
    {
        switch (id)
        {
            case 1:
            {
                descriptiveText1.text = "A quick race across time to reach the boss!";
                break;
            }
            
            case 3:
            {
                descriptiveText1.text = "Need certain fixes for your experience?";
                break;
            }

            case 4:
            {
                descriptiveText1.text = "Need to quit?";
                break;
            }

            case 5:
            {
                diffOptionIndicator.SetActive(true);
                break;
            }

            case 6:
            {
                diffOptionIndicator.SetActive(false);
                break;
            }
            

            default:
            {
                descriptiveText1.text = "";
                break;
            }
        }
    }

    private void LastGameObject()
    {
        
    }

    //Simple Function that is used to update the user display depending on the device being used
    private void UpdateUserInputDisplay()
    {
        //Determine the device that is being used.
        if (playerInput.currentControlScheme == "Controller")
        {
            isGamepad = true;
            isKeyboard = false;
            startButtonText.text = "Press     to Start";
            submitButtonIcon.gameObject.SetActive(true);
            Cursor.visible = false;

            if (menus[0].gameObject.activeInHierarchy && startButton.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(startButton);
            }
        }
        else if(playerInput.currentControlScheme == "Keyboard")
        {
            isKeyboard = true;
            isGamepad = false;
            submitButtonIcon.gameObject.SetActive(false);
            startButtonText.text = "Click to start";
            //EventSystem.current.SetSelectedGameObject(null);
            Cursor.visible = true;
        }
        else
        {
            startButtonText.text = playerInput.currentControlScheme;
            
        }
    }

    public void HoverRotate()
    {
        Debug.Log("You are hovering over object");
    }

    public void HoverRotateExit()
    {
        Debug.Log("You are not hovering over object");
    }

    public void OnSelectVehicle()
    {
        //ToggleMenu(3);

        availableSpaceJets.Remove(currentSpaceJet); //Remove current spaceJet

        var randIndex = Random.Range(0, availableSpaceJets.Count);

        enemySpaceJet = availableSpaceJets[randIndex];

        switch (currentStageId)
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

    //Function to set the difficulty level of the game
    public void DifficultySetting(int difficultyVar)
    {
        switch (difficultyVar)
        {
            //Easy
            case 0:
            {
                reachLimit = 22500;  
                break;
            }

            //Medium
            case 1:
            {
                reachLimit = 26000;
                break;
            }

            //Hard
            case 2:
            {
                reachLimit = 30000;
                break;
            }

            default:
            {
                reachLimit = scaleLevel * 600;
                break;
            }
        }
        
        difficultyLevel = difficultyVar;
        ToggleMenu(3);
        
        rButton.SetActive(true);
        
        // //If device is a gamepad
        // if (isGamepad)
        // {
        //     EventSystem.current.SetSelectedGameObject(rButton);
        // }
        
        EventSystem.current.SetSelectedGameObject(rButton);
        
        DisplayStats();
    }
    

    public void ToggleLeft()
    {
        if (currentId > 0)
        {
            currentId--;
        }

        if (currentId < spaceJets.Length - 1)
        {
            rButton.SetActive(true);
        }

        if (currentId <= 0)
        {
            lButton.SetActive(false);
            rButton.SetActive(true);
        }
        
        ButtonInactivity();

        DisplayStats();
    }

    public void ToggleRight()
    {
        if (currentId < spaceJets.Length)
        {
            currentId++;
        }

        if (currentId > 0)
        {
            lButton.SetActive(true);
        }

        if (currentId == spaceJets.Length - 1)
        {
            rButton.SetActive(false);
        }
        
        ButtonInactivity();
        

        DisplayStats();
    }

    public void OnSelectButtonHighlighted(bool status)
    {
        selectVehicleIndicator.SetActive(status);
    }

    private void DisplayStats()
    {
        currentSpaceJet = spaceJets[currentId];

        for (int i = 0; i < prefabSpaceJets.Length; i++)
        {
            if (i != currentSpaceJet.referenceIndex)
            {
                prefabSpaceJets[i].SetActive(false);
            }
            else
            {
                prefabSpaceJets[i].SetActive(true);
            }
        }

        spaceJetNameText.text = currentSpaceJet.name;

        //Displaying the corresponding ability
        switch (currentSpaceJet.name)
        {
            case "Absorber":
            {
                specialAbilityText.text = "Special Ability: Transmogrifier";
                specialAbilityDesc.text = currentSpaceJet.name +
                                          " gathers materials from the area around the player and" +
                                          " is able to transmogrify it into different powerUps.";
                break;
            }
            
            case "UFO": 
            {
                specialAbilityText.text = "Special Ability: UFO";
                specialAbilityDesc.text = currentSpaceJet.name +
                                          " shares the same name as its ability. What is the true" +
                                          " power of these aliens?";
                break;
            }
            
            case "Bolt Glider": 
            {
                specialAbilityText.text = "Special Ability: ElectroShift";
                specialAbilityDesc.text = currentSpaceJet.name +
                                          " is able to shift into gear modes that increase its speed " +
                                          "and also change the properties that can give it an advantage in a dire situation"; 
                break;
            }
            
            case "Ghost Rider": 
            {
                specialAbilityText.text = "Special Ability: Invisible";
                specialAbilityDesc.text = currentSpaceJet.name +
                                          " is able to turn invisible and distort the area around the player, giving full" +
                                          " immunity to damage for a few seconds and also slow down nearby opponents"; 
                break;
            }
        }


        //Calculate the speedStat UI Display
        var speedVal = (currentSpaceJet.speed / 35000f) * speedStatImg.Length;

        for (int i = 0; i < speedStatImg.Length; i++)
        {
            speedStatImg[i].color = i < (int)speedVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var shieldVal = (currentSpaceJet.shield / 500f) * shieldStatImg.Length;

        for (int i = 0; i < shieldStatImg.Length; i++)
        {
            shieldStatImg[i].color = i < (int)shieldVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var shieldRateVal = (currentSpaceJet.shieldRate / 50f) * shieldRateStatImg.Length;

        for (int i = 0; i < shieldRateStatImg.Length; i++)
        {
            shieldRateStatImg[i].color =
                i < (int)shieldRateVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var gripVal = (currentSpaceJet.grip / 10f) * gripStatImg.Length;

        for (int i = 0; i < gripStatImg.Length; i++)
        {
            gripStatImg[i].color = i < (int)gripVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var thrustVal = (currentSpaceJet.thrust / 300f) * thrustStatImg.Length;

        for (int i = 0; i < thrustStatImg.Length; i++)
        {
            thrustStatImg[i].color = i < (int)thrustVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }

        var laserVal = (currentSpaceJet.laserDamage / 50f) * laserStatImg.Length;

        for (int i = 0; i < laserStatImg.Length; i++)
        {
            laserStatImg[i].color = i < (int)laserVal ? Color.yellow : new Color(0.227451f, 0.227451f, 0.227451f);
        }
    }

    //Function to toggle between menus
    void ToggleMenu(int position)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].gameObject.SetActive(i == position);
        }
    }

    public void ToggleLevelMenu(int position)
    {
        switch (position)
        {
            case 0:
            {
                level1Indicator.SetActive(true);
                level2Indicator.SetActive(false);
                bossNameText.text = "Boss: Pac-Eater";
                break;
            }

            case 1:
            {
                level1Indicator.SetActive(false);
                level2Indicator.SetActive(true);
                bossNameText.text = "Boss: Candy King";
                break;
            }

            default:
            {
                level1Indicator.SetActive(false);
                level2Indicator.SetActive(false);
                bossNameText.text = "Boss: ???";
                break;
            }
        }
    }
    
    //Function to toggle between specific menus
    void ToggleStartMenu(int position)
    {
        switch (position)
        {
            case 0:
            {
                startButton.SetActive(true);
                foreach (var t in menuOptions)
                {
                    t.gameObject.SetActive(false);
                }
                break;
            }

            case 1:
            {
                startButton.SetActive(false);
                foreach (var t in menuOptions)
                {
                    t.gameObject.SetActive(true);
                }
                
                
                break;
            }
        }
    }

    //When the play button is pressed.
    public void OnPlayClicked()
    {
        //SceneManager.LoadScene("StarLightRacers_BetaTest");
        ToggleMenu(1);
        
        // if (playerInput.currentControlScheme == "Controller")
        // {
        //     EventSystem.current.SetSelectedGameObject(difMenuOptions[1].gameObject);
        // }
        
        EventSystem.current.SetSelectedGameObject(level1);
        
    }

    public void OnLevelClicked(int id)
    {
        currentStageId = id;
        ToggleMenu(2);
        EventSystem.current.SetSelectedGameObject(difMenuOptions[0].gameObject);
    }

    private void ButtonInactivity()
    {
        if (lButton.activeInHierarchy == false && rButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(rButton);
        }
        else if (rButton.activeInHierarchy == false && lButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(lButton);
        }
    }
    
    
}
