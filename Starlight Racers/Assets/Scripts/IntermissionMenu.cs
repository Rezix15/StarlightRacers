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

    private int randIndex;
    
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
            randIndex = Random.Range(0, components.Length);
            componentsCards[i] = components[randIndex];
            ComponentDisplay(i);
        }
        
        currentComponent = componentsCards[0];
    }

    public void ContinueSelected()
    {
        SceneManager.LoadScene("StarLightRacers_BetaTest");
    }
}
