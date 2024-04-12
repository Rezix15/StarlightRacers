using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VehicleSelect : MonoBehaviour
{
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
    
    public static SpaceJetObj currentSpaceJet;
    public static SpaceJetObj enemySpaceJet;
    
    public static int currentStageId;
    
    private List<SpaceJetObj> availableSpaceJets; //Array that stores all the remaining spacejets when allocating a random one

    public TextMeshProUGUI spaceJetNameText;
    
    //Indicator to show what slide is current on
    private int currentId = 0;

    [SerializeField] private GameObject lButton;
    [SerializeField] private GameObject rButton;
    [SerializeField] private Button selectButton;

    public GameObject prefabSpaceJetHolder;

    private GameObject[] prefabSpaceJets;
    
    # endregion
    
    
    
    public TextMeshProUGUI specialAbilityText;
    public TextMeshProUGUI specialAbilityDesc;
    public GameObject selectVehicleIndicator;

    public SpaceJetObj[] spaceJets; //Array that stores all the spacejets 

    private void Awake()
    {
        
        var leftButton = lButton.GetComponent<Button>();
        var rightButton = rButton.GetComponent<Button>();
        
        leftButton.onClick.AddListener((() =>
        {
            ToggleLeft();
        }));
        
        rightButton.onClick.AddListener((() =>
        {
            ToggleRight();
        }));
        
        selectButton.onClick.AddListener(() =>
        {
            OnSelectVehicle();
        });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        speedStatImg = speedStat.GetComponentsInChildren<Image>();
        shieldStatImg = shieldStat.GetComponentsInChildren<Image>();
        shieldRateStatImg = shieldRateStat.GetComponentsInChildren<Image>();
        gripStatImg = gripStat.GetComponentsInChildren<Image>();
        thrustStatImg = thrustStat.GetComponentsInChildren<Image>();
        laserStatImg = laserStat.GetComponentsInChildren<Image>();

        currentSpaceJet = spaceJets[0];

        var childCount = prefabSpaceJetHolder.transform.childCount;

        prefabSpaceJets = new GameObject[childCount];
        
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
    
        DisplayStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectVehicle()
    {
        //ToggleMenu(3);
        Debug.Log("Select Vehicle lmao");
        VehicleSelectReady.Instance.SetPlayerReady();
    }

    private void ToggleLeft()
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

    private void ToggleRight()
    {
        if (currentId < spaceJets.Length - 1)
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
