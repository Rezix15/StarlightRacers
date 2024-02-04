using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Component", menuName = "Component")]
public class ComponentObj : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
    };
    
    public new string name;
    public Rarity componentRarity;
    public Image icon;
    public string textDescription;
    

}
