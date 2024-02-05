using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Component", menuName = "ComponentSkill")]
public class ComponentObj : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    };
    
    //The corresponding stat the component is meant to affect
    public enum StatSkillType
    {
        Speed,
        Shield,
        ShieldRate,
        Grip,
        Thrust,
        LaserDamage,
        All,
    };
    
    public new string name;
    public Rarity componentRarity;
    public StatSkillType targetStat;
    public Sprite icon;
    // public string textDescription;
    public float statModifierVal;

}
