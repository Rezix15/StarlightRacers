using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionMenu : MonoBehaviour
{
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
    
    // Start is called before the first frame update
    void Start()
    {
        componentsCards = new ComponentObj[3];
        RollComponents();
    }

    private void ComponentDisplay(int index)
    {
        componentNames[index].text = componentsCards[index].name;
        componentTypes[index].text = componentsCards[index].componentRarity.ToString();
        componentIcons[index].sprite = componentsCards[index].icon;
        skillDescs[index].text = "Boost vehicle " + componentsCards[index].targetStat.ToString().ToLower() + " by " + (componentsCards[index].statModifierVal * 100) +"%";
    }

    // Update is called once per frame
    void Update()
    {
        
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
    

    public void ContinueSelected()
    {
        SceneManager.LoadScene("StarLightRacers_BetaTest");
        MenuManager.componentBoosts.Add(componentsCards[selector]);
    }
}
