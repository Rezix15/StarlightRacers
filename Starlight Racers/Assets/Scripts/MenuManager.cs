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

    public static int RaceCount;
    
    public static List<ComponentObj> componentBoosts;
    
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

    private RaceManager RaceManager;

    public Image[] indicators;

    public Image[] difficultyMenuIndicators;

    public Button[] menuOptions;

    public Button[] difMenuOptions;

    private int menuIndex;

    public static float totalFinishTime;

    public GameObject startButton;

    private TextMeshProUGUI startButtonText; //Text that is used to ask for user Input

    private bool isKeyboard; //Bool to check whether input is keyboard
    private bool isGamepad; //Bool to check whether input is gamepad

    private PlayerInput playerInput;

    private GameObject lastHoveredObj;

    public Image submitButtonIcon;

    public static int difficultyLevel; //global variable that will be used to define the difficulty level for our game.
    public static int scaleLevel; //global variable that will set the scale of the track.
    public static int reachLimit; //global variable that sets the maximum limit co-ordinate the track gen can go

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
        Controller.Disable();
    }
    // Start is called before the first frame update



    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        ToggleMenu(0);
        ToggleStartMenu(0);
        scaleLevel = 45;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUserInputDisplay();
        ToggleThroughMenu();
        ToggleThroughDifficultyMenu();
        
    }

    public void StartGame()
    {
        //ToggleMenu(1);
        
        ToggleStartMenu(1);
        
        if (playerInput.currentControlScheme == "Controller")
        {
            EventSystem.current.SetSelectedGameObject(menuOptions[0].gameObject);
        }
    }

    public void ExitMenu()
    {
        ToggleStartMenu(0);
        
        if (playerInput.currentControlScheme == "Controller")
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }
    
    

    // private void WaitForKeyPress()
    // {
    //     if (isActionPressed && !hasBeenPressed)
    //     {
    //         ToggleMenu(1);
    //         hasBeenPressed = true;
    //         EventSystem.current.SetSelectedGameObject(menuOptions[0].gameObject);
    //     }
    // }

    private void ToggleThroughMenu()
    {
        GameObject selectedOpt = EventSystem.current.currentSelectedGameObject;
        
        if (isGamepad)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                indicators[i].gameObject.SetActive(menuOptions[i].gameObject == selectedOpt);
            }
        }
        else
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                indicators[i].gameObject.SetActive(false);
            }
        }
        
    }

    private void ToggleThroughDifficultyMenu()
    {
        GameObject selectedOpt = EventSystem.current.currentSelectedGameObject;
        
        if (isGamepad)
        {
            for (int i = 0; i < difMenuOptions.Length; i++)
            {
                difficultyMenuIndicators[i].gameObject.SetActive(difMenuOptions[i].gameObject == selectedOpt);
            }
        }
        else
        {
            for (int i = 0; i < difMenuOptions.Length; i++)
            {
                difficultyMenuIndicators[i].gameObject.SetActive(false);
            }
        }
    }
    
    

    //Function that causes the descriptive text to change depending on the id of the button
    public void HoverButton(int id)
    {
        switch (id)
        {
            case 1:
            {
                descriptiveText1.text = "Race against opponents in a single-player Grand Prix.";
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
                difDescriptiveText.text = 
                    "Great for new beginners, who are unfamiliar with the mechanics. ";
                break;
            }

            case 6:
            {
                difDescriptiveText.text =
                    "Great for players who are not looking for an easy challenge but also not a difficult one.";
                break;
            }

            case 7:
            {
                difDescriptiveText.text =
                    "For players who seek a great challenge and experience the true thrill of racing. ";
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
            EventSystem.current.SetSelectedGameObject(null);
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
        RaceCount = 1;

        availableSpaceJets.Remove(currentSpaceJet); //Remove current spaceJet

        var randIndex = Random.Range(0, availableSpaceJets.Count);

        enemySpaceJet = availableSpaceJets[randIndex];
        
        SceneManager.LoadScene("IntermissionScene");
    }

    //Function to set the difficulty level of the game
    public void DifficultySetting(int difficultyVar)
    {
        switch (difficultyVar)
        {
            case 0:
            {
                reachLimit = scaleLevel * 600;
                break;
            }

            case 1:
            {
                reachLimit = scaleLevel * 666;
                break;
            }

            case 2:
            {
                reachLimit = scaleLevel * 800;
                break;
            }

            default:
            {
                reachLimit = scaleLevel * 600;
                break;
            }
        }
        
        difficultyLevel = difficultyVar;
        ToggleMenu(2);
        
        rButton.SetActive(true);
        
        //If device is a gamepad
        if (isGamepad)
        {
            EventSystem.current.SetSelectedGameObject(rButton);
        }
        
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


        //Calculate the speedStat UI Display
        var speedVal = (currentSpaceJet.speed / 40000f) * speedStatImg.Length;

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
        
        if (playerInput.currentControlScheme == "Controller")
        {
            EventSystem.current.SetSelectedGameObject(difMenuOptions[1].gameObject);
        }
    }

    private void ButtonInactivity()
    {
        if (lButton.activeInHierarchy == false && rButton.activeInHierarchy && isGamepad)
        {
            EventSystem.current.SetSelectedGameObject(rButton);
        }
        else if (rButton.activeInHierarchy == false && lButton.activeInHierarchy && isGamepad)
        {
            EventSystem.current.SetSelectedGameObject(lButton);
        }
        
    }
}
