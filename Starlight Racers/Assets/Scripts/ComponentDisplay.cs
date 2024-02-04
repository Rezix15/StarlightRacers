using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComponentDisplay : MonoBehaviour
{
    public TextMeshProUGUI componentName;
    public TextMeshProUGUI componentType;
    public Image componentIcon;
    public TextMeshProUGUI skillDesc;

    private ComponentObj currentComponent;

    public ComponentObj components;
    // Start is called before the first frame update
    void Start()
    {
        componentName.text = components.name;
        componentType.text = components.componentRarity.ToString();
        componentIcon.sprite = components.icon;
        skillDesc.text = "Boost vehicle " + components.targetStat.ToString().ToLower() + " by " + (components.statModifierVal * 100) +"%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
