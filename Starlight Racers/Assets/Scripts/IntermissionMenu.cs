using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Random = UnityEngine.Random;

public class IntermissionMenu : MonoBehaviour
{
    public int cardSeed = 0;
    public GameObject upgradeComp; //Going to be the first component Selected
    public static ComponentObj currentComponent;

    public ComponentObj[] components;

    public ComponentObj[] componentsCards;
    
    public TextMeshProUGUI[] componentNames;
    public TextMeshProUGUI[] componentTypes;
    public Image[] componentIcons;
    public TextMeshProUGUI[] skillDescs;

    public Sprite speedIcon;
    public Sprite shieldIcon;
    public Sprite shieldRateIcon;
    public Sprite laserDmgIcon;
    public Sprite defaultIcon;

    private int randIndex;

    private int selector = 0;

    public GameObject continueBtn;

    public GameObject rerollBtn;

    private TextMeshProUGUI continueText;

    private int rerollMax; //The amount of times the player is allowed to reroll
    private int currentRerolls; 
    
    private string dialogue1;
    
    public bool generateRandomSeed = true;

    private void Awake()
    {
        GenerateCardSeed();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (GameDataManager.RaceCount > 1)
        {
            EventSystem.current.SetSelectedGameObject(upgradeComp);
        }
        
        currentRerolls = 0;
        SetRerolls();
        componentsCards = new ComponentObj[3];
        RollComponents();
        //DisplayContinueText();
        dialogue1 = "Thanks for your purchase. I hope I can be of benefit to you and good luck on your race";
        //Debug.Log("CurrentSelectedObj: " + EventSystem.current.currentSelectedGameObject.name);
    }
    
    private void GenerateCardSeed()
    {
        if (generateRandomSeed)
        {
            cardSeed = (int)System.DateTime.Now.Ticks;
        }
        
        Random.InitState(cardSeed);
    }


    private void ComponentDisplay(int index)
    {
        componentNames[index].text = componentsCards[index].name;
        componentTypes[index].text = componentsCards[index].componentRarity.ToString();
        componentIcons[index].sprite = componentsCards[index].icon;
        skillDescs[index].text = "Boost vehicle " + componentsCards[index].targetStat.ToString().ToLower() + " by " + (componentsCards[index].statModifierVal * 100) +"%";
    }

    // void DisplayContinueText()
    // {
    //     continueText = continueBtn.GetComponentInChildren<TextMeshProUGUI>();
    //
    //     if (GameDataManager.RaceCount > 0)
    //     {
    //         continueText.text = "Start Race";
    //     }
    //     else if(GameDataManager.RaceCount < 3)
    //     {
    //         continueText.text = "Next Race";
    //     }
    //     else if(GameDataManager.RaceCount == 3)
    //     {
    //         continueText.text = "Final Race";
    //     }
    //     else
    //     {
    //         continueText.text = "??? Race";
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("CurrentSelectedObj: " + EventSystem.current.currentSelectedGameObject.name);
        rerollBtn.SetActive(currentRerolls < rerollMax); //Sets the reroll button to be active or inactive depending on how many rerolls have been done
    }

    private void SetRerolls()
    {
        switch (MenuManager.difficultyLevel)
        {
            //If difficulty is at easy mode
            case 0:
            {
                rerollMax = 3;
                break;
            }
            
            //If difficulty is at normal mode
            case 1:
            {
                rerollMax = 1;
                break;
            }
            
            //If difficulty is at hard mode
            case 2:
            {
                rerollMax = 0;
                break;
            }
        }
    }

    public void RollComponents()
    {
        for (int i = 0; i < 3; i++)
        {
            randIndex = Random.Range(0, 2000);
            componentsCards[i] = GenerateComponents(randIndex);
            ComponentDisplay(i);
        }
        
        currentComponent = componentsCards[0];
        currentRerolls++;
    }

    private ComponentObj GenerateComponents(int index)
    {
        int randomStatIndex = Random.Range(0, 6);
        int thresholdVal = Random.Range(1, 5);

        float modifierVal = 0;

        ComponentObj componentCard = new ComponentObj(null, ComponentObj.Rarity.Common, ComponentObj.StatSkillType.All, null, 0);

        var rarity = ComponentObj.Rarity.Common;

        //Component Rarity
        if (index >= 0 && index <= 1000) //50%
        {
            rarity = ComponentObj.Rarity.Common;
            modifierVal = thresholdVal * 0.05f;
        }
        else if (index > 1000 && index <= 1680) //34% chance
        {
            rarity = ComponentObj.Rarity.Uncommon;
            modifierVal = (thresholdVal * 0.05f + 0.2f);
        }
        else if (index > 1680 && index <= 1880) //10% chance
        {
            rarity = ComponentObj.Rarity.Rare;
            modifierVal = (thresholdVal * 0.05f + 0.4f);
        }
        else if (index > 1880 && index <= 1980) //5% chance
        {
            rarity = ComponentObj.Rarity.Epic;
            modifierVal = (thresholdVal * 0.05f + 0.6f);
        }
        else if (index > 1980 && index <= 2000) //1% chance
        {
            rarity = ComponentObj.Rarity.Legendary;
            modifierVal = (thresholdVal * 0.05f + 0.8f);
        }
        else
        {
            rarity = ComponentObj.Rarity.Common;
        }
        
        switch (randomStatIndex)
        {
            case 0:
            {
                componentCard = new ComponentObj("Turbo Engine", rarity, ComponentObj.StatSkillType.Speed, speedIcon, modifierVal);
                break;
            }
            
            case 1:
            {
                componentCard = new ComponentObj("Energy Component", rarity, ComponentObj.StatSkillType.Shield, shieldIcon, modifierVal);
                break;
            }
            
            case 2:
            {
                componentCard = new ComponentObj("Defense Shields", rarity, ComponentObj.StatSkillType.ShieldRate, shieldRateIcon, modifierVal);
                break;
            }
            
            case 3:
            {
                componentCard = new ComponentObj("Space Locks", rarity, ComponentObj.StatSkillType.Grip, defaultIcon, modifierVal);
                break;
            }
            
            case 4:
            {
                componentCard = new ComponentObj("High-Energy Thrusters", rarity, ComponentObj.StatSkillType.Thrust, defaultIcon, modifierVal);
                break;
            }
            
            case 5:
            {
                componentCard = new ComponentObj("Laser Cannons", rarity, ComponentObj.StatSkillType.LaserDamage, laserDmgIcon, modifierVal);
                break;
            }

            default:
            {
                
                break;
            }
        }

        return componentCard;
    }

    public void SelectCard(int id)
    {
        selector = id;
    }

    public void ClickCard()
    {
        EventSystem.current.SetSelectedGameObject(continueBtn);
    }

    //Player exits out
    public void Exit()
    {
        BoosterShopkeeper.ActivateBoosterMenu = false;
    }


    IEnumerator EndingMessage(string dialogue)
    {
        DialogueManager.currentDialogue = new Dialogue("Cleric1", dialogue, Dialogue.DialogueType.Text);
        yield return new WaitForSeconds(2);
        DialogueManager.inDialogue = false;
        BoosterShopkeeper.prevRaceCount = GameDataManager.RaceCount;
    }
    
    public void ContinueSelected()
    {
        MenuManager.componentBoosts.Add(componentsCards[selector]);
        BoosterShopkeeper.ActivateBoosterMenu = false;
        BoosterShopkeeper.successfulAttempts++;
        StartCoroutine(EndingMessage(dialogue1));
    }
}
